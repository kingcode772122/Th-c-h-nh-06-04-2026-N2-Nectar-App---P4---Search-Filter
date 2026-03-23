import { View, Text, StyleSheet, Image, TextInput, TouchableOpacity } from "react-native";
import { useRouter } from "expo-router";
export default function SignIn() {
    const router = useRouter();
  return (
    <View style={styles.container}>
      
      {/* ẢNH HEADER */}
      <Image
        source={require("../assets/Mask Group.png")} // 👉 bạn thay link ảnh ở đây
        style={styles.headerImage}
      />

      {/* CONTENT */}
      <View style={styles.content}>
        <Text style={styles.title}>
          Get your groceries{"\n"}with nectar
        </Text>

        {/* INPUT PHONE */}
        <View style={styles.inputRow}>
          <Text style={styles.flag}>🇧🇩</Text>
          <TextInput
            placeholder="+880"
            placeholderTextColor="#000"
            style={styles.input}
          />
        </View>

        <View style={styles.line} />

        <Text style={styles.orText}>Or connect with social media</Text>

        {/* GOOGLE */}
        <TouchableOpacity style={[styles.btn, styles.google]}
        onPress={() => router.replace("/number")}>
          <Text style={styles.btnText}>Continue with Google</Text>
        </TouchableOpacity>

        {/* FACEBOOK */}
        <TouchableOpacity style={[styles.btn, styles.facebook]}>
          <Text style={styles.btnText}>Continue with Facebook</Text>
        </TouchableOpacity>

      </View>
    </View>
  );
}   
const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#fff",
  },

  headerImage: {
    width: "100%",
    height: 300,
    resizeMode: "cover",
  },

  content: {
    flex: 1,
    paddingHorizontal: 25,
    marginTop: 20,
  },

  title: {
    fontSize: 26,
    fontWeight: "600",
    marginBottom: 30,
  },

  inputRow: {
    flexDirection: "row",
    alignItems: "center",
  },

  flag: {
    fontSize: 20,
    marginRight: 10,
  },

  input: {
    fontSize: 16,
  },

  line: {
    height: 1,
    backgroundColor: "#ccc",
    marginTop: 10,
    marginBottom: 30,
  },

  orText: {
    textAlign: "center",
    color: "#888",
    marginBottom: 20,
  },

  btn: {
    paddingVertical: 18,
    borderRadius: 15,
    alignItems: "center",
    marginBottom: 15,
  },

  google: {
    backgroundColor: "#5383EC",
  },

  facebook: {
    backgroundColor: "#4A66AC",
  },

  btnText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "500",
  },
});