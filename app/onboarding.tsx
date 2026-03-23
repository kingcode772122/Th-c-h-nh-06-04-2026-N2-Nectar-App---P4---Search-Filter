import { View, Text, StyleSheet, Image, TouchableOpacity } from "react-native";
import { useRouter } from "expo-router";

export default function Onboarding() {
  const router = useRouter();

  return (
    <View style={styles.container}>
      <Image
        source={require("../assets/8140 1.png")}
        style={styles.image}
        
      />

      <View style={styles.overlay}>
        <Text style={styles.title}>
          Welcome{"\n"}to our store
        </Text>

        <Text style={styles.desc}>
          Get your groceries in as fast as one hour
        </Text>

        <TouchableOpacity
          style={styles.button}
          onPress={() => router.replace("/signin")}
        >
          <Text style={styles.btnText}>Get Started</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },

  image: {
    width: "100%",
    height: "100%",
    position: "absolute",
  },

  overlay: {
    flex: 1,
    justifyContent: "flex-end",
    alignItems: "center",
    paddingBottom: 60,
    backgroundColor: "rgba(0,0,0,0.3)",
  },

  icon: {
    fontSize: 40,
    marginBottom: 10,
  },

  title: {
    fontSize: 30,
    color: "#fff",
    fontWeight: "bold",
    textAlign: "center",
  },

  desc: {
    color: "#ddd",
    marginVertical: 10,
  },

  button: {
    backgroundColor: "#53B175",
    paddingVertical: 15,
    paddingHorizontal: 80,
    borderRadius: 20,
    marginTop: 20,
  },

  btnText: {
    color: "#fff",
    fontSize: 16,
  },
});