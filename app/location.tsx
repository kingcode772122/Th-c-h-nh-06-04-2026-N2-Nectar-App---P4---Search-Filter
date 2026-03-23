import React from "react";
import {
  View,
  Text,
  TouchableOpacity,
  StyleSheet,
  SafeAreaView,
  Image,
} from "react-native";
import { useRouter } from "expo-router";
export default function LocationScreen() {
  const router = useRouter();
  return (
    <SafeAreaView style={styles.container}>
      {/* BACK */}
      <TouchableOpacity style={styles.back}
      onPress={() => router.replace("/verification")}>
        <Text style={styles.backIcon}>‹</Text>
      </TouchableOpacity>

      {/* IMAGE */}
      <Image
        source={require("../assets/illustration.png")} // 👉 bạn thay ảnh ở đây
        style={styles.image}
        resizeMode="contain"
      />

      {/* TITLE */}
      <Text style={styles.title}>Select Your Location</Text>

      {/* SUB */}
      <Text style={styles.subtitle}>
        Switch on your location to stay in tune with{"\n"}
        what’s happening in your area
      </Text>

      {/* FORM */}
      <View style={styles.form}>
        {/* Zone */}
        <Text style={styles.label}>Your Zone</Text>
        <View style={styles.inputRow}>
          <Text style={styles.inputText}>Banasree</Text>
          <Text style={styles.arrow}>⌄</Text>
        </View>

        {/* Area */}
        <Text style={[styles.label, { marginTop: 25 }]}>
          Your Area
        </Text>
        <View style={styles.inputRow}>
          <Text style={styles.placeholder}>Types of your area</Text>
          <Text style={styles.arrow}>⌄</Text>
        </View>
      </View>

      {/* BUTTON */}
      <TouchableOpacity style={styles.button}
      onPress={() => router.replace("/login")}>
        <Text style={styles.buttonText}>Submit</Text>
      </TouchableOpacity>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f2f2f2",
    paddingHorizontal: 20,
  },

  back: {
    marginTop: 10,
    marginBottom: 10,
  },

  backIcon: {
    fontSize: 28,
    color: "#111",
  },

  image: {
    width: "100%",
    height: 180,
    marginTop: 10,
    marginBottom: 20,
  },

  title: {
    fontSize: 22,
    fontWeight: "600",
    textAlign: "center",
    color: "#222",
    marginBottom: 10,
  },

  subtitle: {
    fontSize: 14,
    color: "#777",
    textAlign: "center",
    lineHeight: 20,
    marginBottom: 30,
  },

  form: {
    marginTop: 200,
  },

  label: {
    fontSize: 13,
    color: "#999",
    marginBottom: 8,
  },

  inputRow: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    borderBottomWidth: 1,
    borderBottomColor: "#ddd",
    paddingBottom: 10,
  },

  inputText: {
    fontSize: 16,
    color: "#111",
  },

  placeholder: {
    fontSize: 16,
    color: "#bbb",
  },

  arrow: {
    fontSize: 16,
    color: "#777",
  },

  button: {
    position: "absolute",
    bottom: 80,
    left: 20,
    right: 20,
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
});