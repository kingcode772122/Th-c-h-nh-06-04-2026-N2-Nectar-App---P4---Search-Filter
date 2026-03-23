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

export default function SignupScreen() {
  const [showPass, setShowPass] = useState(false);

  return (
    <SafeAreaView style={styles.container}>
      {/* LOGO */}
      <Image
        source={require("../assets/Group.png")}
        style={styles.logo}
        resizeMode="contain"
      />

      {/* TITLE */}
      <Text style={styles.title}>Sign Up</Text>
      <Text style={styles.subtitle}>
        Enter your credentials to continue
      </Text>

      {/* USERNAME */}
      <Text style={styles.label}>Username</Text>
      <TextInput
        style={styles.input}
        defaultValue="Afsar Hossen Shuvo"
      />

      {/* EMAIL */}
      <Text style={styles.label}>Email</Text>
      <View style={styles.inputRow}>
        <TextInput
          style={styles.inputFlex}
          defaultValue="imshuvo97@gmail.com"
        />
        <Text style={styles.check}>✓</Text>
      </View>

      {/* PASSWORD */}
      <Text style={styles.label}>Password</Text>
      <View style={styles.inputRow}>
        <TextInput
          style={styles.inputFlex}
          secureTextEntry={!showPass}
          defaultValue="12345678"
        />
        <TouchableOpacity onPress={() => setShowPass(!showPass)}>
          <Text style={styles.eye}>👁</Text>
        </TouchableOpacity>
      </View>

      {/* TERMS */}
      <Text style={styles.terms}>
        By continuing you agree to our{" "}
        <Text style={styles.link}>Terms of Service</Text> and{" "}
        <Text style={styles.link}>Privacy Policy</Text>.
      </Text>

      {/* BUTTON */}
      <TouchableOpacity style={styles.button}>
        <Text style={styles.buttonText}>Sign Up</Text>
      </TouchableOpacity>

      {/* LOGIN */}
      <Text style={styles.login}>
        Already have an account?{" "}
        <Text style={styles.loginHighlight}>Signup</Text>
      </Text>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    paddingHorizontal: 24,
    justifyContent: "center",
    backgroundColor: "#f5f5f5",
  },

  logo: {
    width: 60,
    height: 60,
    alignSelf: "center",
    marginBottom: 20,
  },

  title: {
    fontSize: 28,
    fontWeight: "600",
    marginBottom: 5,
    color: "#111",
  },

  subtitle: {
    fontSize: 14,
    color: "#888",
    marginBottom: 25,
  },

  label: {
    fontSize: 13,
    color: "#999",
    marginTop: 15,
  },

  input: {
    borderBottomWidth: 1,
    borderBottomColor: "#ddd",
    fontSize: 16,
    paddingVertical: 8,
    color: "#111",
  },

  inputRow: {
    flexDirection: "row",
    alignItems: "center",
    borderBottomWidth: 1,
    borderBottomColor: "#ddd",
    paddingVertical: 8,
  },

  inputFlex: {
    flex: 1,
    fontSize: 16,
    color: "#111",
  },

  check: {
    color: "#5BAA73",
    fontSize: 18,
    marginLeft: 5,
  },

  eye: {
    fontSize: 18,
    color: "#999",
  },

  terms: {
    fontSize: 12,
    color: "#888",
    marginTop: 20,
    lineHeight: 18,
  },

  link: {
    color: "#5BAA73",
  },

  button: {
    marginTop: 25,
    height: 55,
    backgroundColor: "#5BAA73",
    borderRadius: 20,
    justifyContent: "center",
    alignItems: "center",
  },

  buttonText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "600",
  },

  login: {
    textAlign: "center",
    marginTop: 15,
    fontSize: 14,
    color: "#555",
  },

  loginHighlight: {
    color: "#5BAA73",
  },
});