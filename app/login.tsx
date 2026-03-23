import React, { useState } from "react";
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  SafeAreaView,
  Image,
} from "react-native";
import { useRouter } from "expo-router";
export default function LoginScreen() {
  const router = useRouter();
  const [passwordVisible, setPasswordVisible] = useState(false);

  return (
    <SafeAreaView style={styles.container}>
      {/* CARROT IMAGE */}
      <Image
        source={require("../assets/Group.png")} // 👉 bạn thêm ảnh vào đây
        style={styles.logo}
        resizeMode="contain"
      />

      {/* TITLE */}
      <Text style={styles.title}>Loging</Text>

      <Text style={styles.subtitle}>
        Enter your emails and password
      </Text>

      {/* EMAIL */}
      <Text style={styles.label}>Email</Text>
      <TextInput
        style={styles.input}
        defaultValue="imshuvo97@gmail.com"
      />

      {/* PASSWORD */}
      <Text style={[styles.label, { marginTop: 20 }]}>
        Password
      </Text>

      <View style={styles.passwordRow}>
        <TextInput
          style={styles.passwordInput}
          secureTextEntry={!passwordVisible}
          defaultValue="12345678"
        />
        <TouchableOpacity
          onPress={() => setPasswordVisible(!passwordVisible)}
        >
          <Text style={styles.eye}>👁️</Text>
        </TouchableOpacity>
      </View>

      {/* FORGOT */}
      <Text style={styles.forgot}>Forgot Password?</Text>

      {/* BUTTON */}
      <TouchableOpacity style={styles.button}>
        <Text style={styles.buttonText}>Log In</Text>
      </TouchableOpacity>

      {/* SIGNUP */}
      <Text style={styles.signup}>
        Don’t have an account?{" "}
        <Text style={styles.signupHighlight} onPress={() => router.replace("/signup")}>
          Singup
        </Text>
      </Text>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f2f2f2",
    paddingHorizontal: 24,
    marginTop:120,
  },

  logo: {
    width: 80,
    height: 80,
    alignSelf: "center",
    marginTop: 30,
    marginBottom: 20,
  },

  title: {
    fontSize: 26,
    fontWeight: "600",
    color: "#222",
    marginBottom: 8,
    marginTop:30,
  },

  subtitle: {
    fontSize: 14,
    color: "#777",
    marginBottom: 30,
  },

  label: {
    fontSize: 13,
    color: "#999",
    marginBottom: 6,
  },

  input: {
    borderBottomWidth: 1,
    borderBottomColor: "#ddd",
    fontSize: 16,
    paddingVertical: 8,
    color: "#111",
  },

  passwordRow: {
    flexDirection: "row",
    alignItems: "center",
    borderBottomWidth: 1,
    borderBottomColor: "#ddd",
  },

  passwordInput: {
    flex: 1,
    fontSize: 16,
    paddingVertical: 8,
    color: "#111",
  },

  eye: {
    fontSize: 18,
    color: "#777",
  },

  forgot: {
    textAlign: "right",
    fontSize: 13,
    color: "#555",
    marginTop: 10,
  },

  button: {
    marginTop: 30,
    height: 55,
    backgroundColor: "#5BAA73",
    borderRadius: 15,
    justifyContent: "center",
    alignItems: "center",
  },

  buttonText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "600",
  },

  signup: {
    textAlign: "center",
    marginTop: 20,
    fontSize: 14,
    color: "#555",
  },

  signupHighlight: {
    color: "#5BAA73",
  },
});