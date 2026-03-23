using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace server
{
    internal class Program
    {
        // Danh sách clients đang online
        static List<Socket> clients = new List<Socket>();
        static Dictionary<Socket, ClientInfo> clientInfo = new Dictionary<Socket, ClientInfo>();

        // Lưu danh sách file của mỗi client
        static Dictionary<string, List<FileInfo>> fileDatabase = new Dictionary<string, List<FileInfo>>();

        // Lưu các yêu cầu tìm file đang chờ
        static Dictionary<string, SearchRequest> pendingSearches = new Dictionary<string, SearchRequest>();

        static object lockObj = new object();

        static void Main(string[] args)
        {
            Socket server = new Socket(AddressFamily.InterNetwork,
                                       SocketType.Stream,
                                       ProtocolType.Tcp);

            server.Bind(new IPEndPoint(IPAddress.Any, 5000));
            server.Listen(100);

            Console.WriteLine("🚀 P2P File Sharing Server đang chạy...");
            Console.WriteLine($"📡 IP: {GetLocalIPAddress()}, Port: 5000");
            Console.WriteLine("========================================");

            // Thread gửi ping định kỳ để kiểm tra kết nối
            Thread pingThread = new Thread(PingAllClients);
            pingThread.IsBackground = true;
            pingThread.Start();

            // Thread xử lý lệnh từ console
            Thread consoleThread = new Thread(() =>
            {
                while (true)
                {
                    string cmd = Console.ReadLine()?.ToLower();
                    if (cmd == "list")
                        ShowAllFiles();
                    else if (cmd == "clients")
                        ShowAllClients();
                }
            });
            consoleThread.IsBackground = true;
            consoleThread.Start();

            while (true)
            {
                Socket client = server.Accept();
                IPEndPoint clientEP = (IPEndPoint)client.RemoteEndPoint;

                Console.WriteLine($"✅ Client mới kết nối: {clientEP.Address}:{clientEP.Port}");

                lock (lockObj)
                {
                    clients.Add(client);
                    clientInfo[client] = new ClientInfo
                    {
                        IP = clientEP.Address.ToString(),
                        Port = clientEP.Port,
                        Files = new List<FileInfo>(),
                        LastSeen = DateTime.Now,
                        IsOnline = true
                    };
                }

                Thread t = new Thread(() => HandleClient(client));
                t.IsBackground = true;
                t.Start();
            }
        }

        static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        static void PingAllClients()
        {
            while (true)
            {
                Thread.Sleep(30000); // Ping mỗi 30 giây

                lock (lockObj)
                {
                    List<Socket> deadClients = new List<Socket>();

                    foreach (var client in clients)
                    {
                        try
                        {
                            Send(client, "PING");
                        }
                        catch
                        {
                            deadClients.Add(client);
                        }
                    }

                    // Xóa clients không phản hồi
                    foreach (var dead in deadClients)
                    {
                        clients.Remove(dead);
                        if (clientInfo.ContainsKey(dead))
                        {
                            string clientIP = clientInfo[dead].IP;
                            string username = clientInfo[dead].Username ?? "Unknown";
                            Console.WriteLine($"🔌 Client {username} ({clientIP}) mất kết nối (timeout)");

                            // Xóa file của client khỏi database
                            if (!string.IsNullOrEmpty(username))
                            {
                                RemoveClientFiles(username);
                            }

                            clientInfo.Remove(dead);
                        }
                    }
                }
            }
        }

        static void HandleClient(Socket client)
        {
            byte[] buffer = new byte[8192];
            string cache = "";

            try
            {
                while (true)
                {
                    int bytes = client.Receive(buffer);

                    if (bytes == 0) break;

                    string received = Encoding.UTF8.GetString(buffer, 0, bytes);
                    cache += received;

                    string[] messages = cache.Split('\n');

                    for (int i = 0; i < messages.Length - 1; i++)
                    {
                        string msg = messages[i].Trim();
                        if (!string.IsNullOrEmpty(msg))
                        {
                            Console.WriteLine($"📩 [{clientInfo[client].IP}] {msg}");
                            ProcessMessage(client, msg);
                        }
                    }

                    cache = messages[^1];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi client: {ex.Message}");
            }
            finally
            {
                lock (lockObj)
                {
                    clients.Remove(client);
                    if (clientInfo.ContainsKey(client))
                    {
                        string clientIP = clientInfo[client].IP;
                        string username = clientInfo[client].Username ?? "Unknown";
                        Console.WriteLine($"👋 Client {username} ({clientIP}) đã ngắt kết nối");

                        // Xóa file của client khỏi database
                        if (!string.IsNullOrEmpty(username))
                        {
                            RemoveClientFiles(username);
                        }

                        clientInfo.Remove(client);
                    }
                }
                client.Close();
            }
        }

        static void RemoveClientFiles(string username)
        {
            if (string.IsNullOrEmpty(username)) return;

            lock (lockObj)
            {
                if (fileDatabase.ContainsKey(username))
                {
                    fileDatabase.Remove(username);
                    Console.WriteLine($"🗑️ Đã xóa file của user {username} khỏi database");
                }
            }
        }

        static void ProcessMessage(Socket client, string msg)
        {
            string[] parts = msg.Split('|');
            if (parts.Length < 1) return;

            string command = parts[0].ToUpper();

            switch (command)
            {
                case "LOGIN":
                    HandleLogin(client, parts);
                    break;

                case "REGISTER_FILES":
                    HandleRegisterFiles(client, parts);
                    break;

                case "SEARCH":
                    HandleSearchFile(client, parts);
                    break;

                case "SEARCH_RESPONSE":
                    HandleSearchResponse(client, parts);
                    break;

                case "GET_PEERS":
                    HandleGetPeers(client, parts);
                    break;

                case "REPORT_CHUNK":
                    HandleReportChunk(client, parts);
                    break;

                case "DOWNLOAD_COMPLETE":
                    HandleDownloadComplete(client, parts);
                    break;

                case "PONG":
                    lock (lockObj)
                    {
                        if (clientInfo.ContainsKey(client))
                        {
                            clientInfo[client].LastSeen = DateTime.Now;
                        }
                    }
                    break;

                case "LOGOUT":
                    HandleLogout(client);
                    break;

                default:
                    Console.WriteLine($"⚠️ Lệnh không xác định: {command}");
                    break;
            }
        }

        static void HandleLogin(Socket client, string[] parts)
        {
            if (parts.Length < 3)
            {
                Send(client, "FAIL|Invalid format");
                return;
            }

            string username = parts[1];
            string password = parts[2];

            if (CheckLogin(username, password))
            {
                lock (lockObj)
                {
                    if (clientInfo.ContainsKey(client))
                    {
                        clientInfo[client].Username = username;
                        clientInfo[client].LoginTime = DateTime.Now;
                    }
                }

                Console.WriteLine($"✅ User {username} đăng nhập thành công từ {clientInfo[client].IP}");
                Send(client, "OK");
            }
            else
            {
                Console.WriteLine($"❌ User {username} đăng nhập thất bại từ {clientInfo[client].IP}");
                Send(client, "FAIL");
            }
        }

        static void HandleRegisterFiles(Socket client, string[] parts)
        {
            string username = null;
            lock (lockObj)
            {
                if (clientInfo.ContainsKey(client))
                {
                    username = clientInfo[client].Username;
                }
            }

            if (string.IsNullOrEmpty(username))
            {
                Send(client, "ERROR|Not logged in");
                return;
            }

            if (parts.Length < 2)
            {
                Send(client, "ERROR|Missing file list");
                return;
            }

            try
            {
                string filesData = parts[1];
                string[] fileEntries = filesData.Split(',');

                List<FileInfo> files = new List<FileInfo>();

                foreach (string entry in fileEntries)
                {
                    string[] fileInfo = entry.Split(':');
                    if (fileInfo.Length >= 3)
                    {
                        files.Add(new FileInfo
                        {
                            Name = fileInfo[0],
                            Size = long.Parse(fileInfo[1]),
                            Hash = fileInfo[2]
                        });
                    }
                }

                lock (lockObj)
                {
                    // Lưu vào database
                    fileDatabase[username] = files;

                    // QUAN TRỌNG: Lưu vào clientInfo
                    if (clientInfo.ContainsKey(client))
                    {
                        clientInfo[client].Files = files;
                    }
                }

                Console.WriteLine($"📦 User {username} đăng ký {files.Count} file");
                Send(client, $"OK|Registered {files.Count} files");
            }
            catch (Exception ex)
            {
                Send(client, $"ERROR|{ex.Message}");
            }
        }

        static void HandleSearchFile(Socket client, string[] parts)
        {
            if (parts.Length < 2)
            {
                Send(client, "ERROR|Missing filename");
                return;
            }

            string filename = parts[1].ToLower();
            string searchId = Guid.NewGuid().ToString().Substring(0, 8);

            List<PeerInfo> peersWithFile = new List<PeerInfo>();

            lock (lockObj)
            {
                // Duyệt qua tất cả client đang online
                foreach (var kvp in clientInfo)
                {
                    var info = kvp.Value;

                    // Chỉ xét client đã login và có file
                    if (!string.IsNullOrEmpty(info.Username) && info.Files != null && info.Files.Count > 0)
                    {
                        // Kiểm tra từng file của client
                        foreach (var file in info.Files)
                        {
                            if (file.Name.ToLower().Contains(filename))
                            {
                                peersWithFile.Add(new PeerInfo
                                {
                                    IP = info.IP,
                                    Port = 5001,
                                    Username = info.Username,
                                    FileName = file.Name,
                                    FileSize = file.Size,
                                    FileHash = file.Hash
                                });
                                break;
                            }
                        }
                    }
                }
            }

            if (peersWithFile.Count > 0)
            {
                string response = "SEARCH_RESULT|" + searchId + "|";
                foreach (var peer in peersWithFile)
                {
                    response += $"{peer.IP}:{peer.Port}:{peer.FileName}:{peer.FileSize}:{peer.FileHash},";
                }
                response = response.TrimEnd(',');

                Send(client, response);
                Console.WriteLine($"🔍 Tìm thấy {peersWithFile.Count} peer có file '{filename}'");
            }
            else
            {
                // Không tìm thấy, broadcast yêu cầu đến các client khác
                lock (lockObj)
                {
                    pendingSearches[searchId] = new SearchRequest
                    {
                        Id = searchId,
                        Filename = filename,
                        Requester = client,
                        Timeout = DateTime.Now.AddSeconds(30),
                        Responses = new List<PeerInfo>()
                    };
                }

                int broadcastCount = 0;
                lock (lockObj)
                {
                    foreach (var c in clients)
                    {
                        if (c != client && clientInfo.ContainsKey(c) && !string.IsNullOrEmpty(clientInfo[c].Username))
                        {
                            Send(c, $"SEARCH_REQUEST|{searchId}|{filename}");
                            broadcastCount++;
                        }
                    }
                }

                Console.WriteLine($"📢 Broadcast yêu cầu tìm '{filename}' đến {broadcastCount} client");

                Thread timeoutThread = new Thread(() => SearchTimeout(searchId));
                timeoutThread.IsBackground = true;
                timeoutThread.Start();
            }
        }

        static void SearchTimeout(string searchId)
        {
            Thread.Sleep(30000);

            lock (lockObj)
            {
                if (pendingSearches.ContainsKey(searchId))
                {
                    var request = pendingSearches[searchId];

                    if (request.Responses.Count > 0)
                    {
                        string response = "SEARCH_RESULT|" + searchId + "|";
                        foreach (var peer in request.Responses)
                        {
                            response += $"{peer.IP}:{peer.Port}:{peer.FileName}:{peer.FileSize}:{peer.FileHash},";
                        }
                        response = response.TrimEnd(',');

                        Send(request.Requester, response);
                    }
                    else
                    {
                        Send(request.Requester, $"SEARCH_RESULT|{searchId}|NOT_FOUND");
                    }

                    pendingSearches.Remove(searchId);
                }
            }
        }

        static void HandleSearchResponse(Socket client, string[] parts)
        {
            if (parts.Length < 6) return;

            string searchId = parts[1];
            string filename = parts[2];
            long fileSize = long.Parse(parts[3]);
            string fileHash = parts[4];
            int p2pPort = int.Parse(parts[5]);

            lock (lockObj)
            {
                if (pendingSearches.ContainsKey(searchId))
                {
                    var request = pendingSearches[searchId];

                    if (clientInfo.ContainsKey(client))
                    {
                        request.Responses.Add(new PeerInfo
                        {
                            IP = clientInfo[client].IP,
                            Port = p2pPort,
                            Username = clientInfo[client].Username,
                            FileName = filename,
                            FileSize = fileSize,
                            FileHash = fileHash
                        });

                        Console.WriteLine($"📥 Peer {clientInfo[client].Username} có file '{filename}'");
                    }
                }
            }
        }

        static void HandleGetPeers(Socket client, string[] parts)
        {
            if (parts.Length < 3) return;

            string filename = parts[1];
            string fileHash = parts[2];

            List<PeerInfo> peers = new List<PeerInfo>();

            lock (lockObj)
            {
                foreach (var kvp in clientInfo)
                {
                    var info = kvp.Value;

                    if (!string.IsNullOrEmpty(info.Username) && info.Files != null)
                    {
                        foreach (var file in info.Files)
                        {
                            if (file.Name == filename && file.Hash == fileHash)
                            {
                                peers.Add(new PeerInfo
                                {
                                    IP = info.IP,
                                    Port = 5001,
                                    Username = info.Username,
                                    FileName = file.Name,
                                    FileSize = file.Size,
                                    FileHash = file.Hash
                                });
                                break;
                            }
                        }
                    }
                }
            }

            if (peers.Count > 0)
            {
                string response = "PEER_LIST|";
                foreach (var peer in peers)
                {
                    response += $"{peer.IP}:{peer.Port},";
                }
                response = response.TrimEnd(',');

                Send(client, response);
            }
            else
            {
                Send(client, "PEER_LIST|EMPTY");
            }
        }

        static void HandleReportChunk(Socket client, string[] parts)
        {
            if (parts.Length < 4) return;

            string filename = parts[1];
            int chunkIndex = int.Parse(parts[2]);
            string downloader = parts[3];

            Console.WriteLine($"🧩 {downloader} đã tải xong chunk {chunkIndex} của {filename}");
        }

        static void HandleDownloadComplete(Socket client, string[] parts)
        {
            if (parts.Length < 3) return;

            string filename = parts[1];
            string downloader = parts[2];

            Console.WriteLine($"✅ {downloader} đã tải xong file {filename}");
        }

        static void HandleLogout(Socket client)
        {
            lock (lockObj)
            {
                if (clientInfo.ContainsKey(client))
                {
                    string username = clientInfo[client].Username;
                    if (!string.IsNullOrEmpty(username))
                    {
                        RemoveClientFiles(username);
                        Console.WriteLine($"👋 User {username} đã đăng xuất");
                    }
                    clientInfo.Remove(client);
                }
            }
            Send(client, "OK|Logged out");
        }

        static void Send(Socket client, string msg)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(msg + "\n");
                client.Send(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi gửi dữ liệu: {ex.Message}");
            }
        }

        static bool CheckLogin(string user, string pass)
        {
            string connStr = "Server=.;Database=SocketApp;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM TAIKHOAN WHERE username=@u AND password=@p";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", user);
                    cmd.Parameters.AddWithValue("@p", pass);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi database: {ex.Message}");
                return false;
            }
        }

        // Hàm hiển thị tất cả file đang chia sẻ
        static void ShowAllFiles()
        {
            Console.WriteLine("\n📋 DANH SÁCH FILE ĐANG CHIA SẺ:");
            Console.WriteLine("=================================");

            lock (lockObj)
            {
                foreach (var kvp in fileDatabase)
                {
                    string username = kvp.Key;
                    List<FileInfo> files = kvp.Value;

                    Console.WriteLine($"👤 User: {username}");
                    foreach (var file in files)
                    {
                        Console.WriteLine($"   📁 {file.Name} ({file.Size} bytes) - {file.Hash.Substring(0, 8)}...");
                    }
                }
            }

            Console.WriteLine("=================================\n");
        }

        // Hàm hiển thị tất cả client đang online
        static void ShowAllClients()
        {
            Console.WriteLine("\n📋 DANH SÁCH CLIENT ONLINE:");
            Console.WriteLine("=============================");

            lock (lockObj)
            {
                foreach (var kvp in clientInfo)
                {
                    var info = kvp.Value;
                    string status = info.IsOnline ? "🟢 Online" : "🔴 Offline";
                    Console.WriteLine($"👤 {info.Username ?? "Unknown"} - {info.IP}:{info.Port} - {status} - Files: {info.Files?.Count ?? 0}");
                }
            }

            Console.WriteLine("=============================\n");
        }
    }

    // Các class hỗ trợ
    class ClientInfo
    {
        public string? IP { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public List<FileInfo> Files { get; set; } = new List<FileInfo>();
        public DateTime LastSeen { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsOnline { get; set; } = true;
    }

    class FileInfo
    {
        public string Name { get; set; } = "";
        public long Size { get; set; }
        public string Hash { get; set; } = "";
    }

    class PeerInfo
    {
        public string IP { get; set; } = "";
        public int Port { get; set; }
        public string Username { get; set; } = "";
        public string FileName { get; set; } = "";
        public long FileSize { get; set; }
        public string FileHash { get; set; } = "";
    }

    class SearchRequest
    {
        public string Id { get; set; } = "";
        public string Filename { get; set; } = "";
        public Socket Requester { get; set; } = null!;
        public DateTime Timeout { get; set; }
        public List<PeerInfo> Responses { get; set; } = new List<PeerInfo>();
    }
}