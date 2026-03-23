import React, { useState } from "react";
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  SafeAreaView,
  Dimensions,
} from "react-native";
import { useRouter } from "expo-router";
const { height } = Dimensions.get("window");

export default function PhoneScreen() {
    const router = useRouter();
  const [phone, setPhone] = useState("");

  const handlePress = (value) => {
    if (value === "del") {
      setPhone(phone.slice(0, -1));
    } else {
      setPhone(phone + value);
    }
  };

  const renderKey = (num, letters = "") => (
    <TouchableOpacity
      style={styles.key}
      onPress={() => handlePress(num)}
      activeOpacity={0.7}
    >
      <Text style={styles.keyNumber}>{num}</Text>
      {letters ? <Text style={styles.keyLetters}>{letters}</Text> : null}
    </TouchableOpacity>
  );


  return (
    <SafeAreaView style={styles.container}>
        
      {/* HEADER */}
      <View style={styles.header}>
        <Text style={styles.title}>Enter your mobile number</Text>

        <Text style={styles.label}>Mobile Number</Text>

        <View style={styles.inputContainer}>
          <Text style={styles.flag}>🇧🇩</Text>
          <Text style={styles.prefix}>+880</Text>
          <TextInput
            style={styles.input}
            value={phone}
            editable={false}
          />
        </View>
      </View>

      {/* FLOAT BUTTON */}
      <TouchableOpacity style={styles.nextBtn}
      onPress={() => router.replace("/verification")}>
        <Text style={styles.arrow}>{">"}</Text>
      </TouchableOpacity>

      {/* KEYPAD */}
      <View style={styles.keypad}>
        <View style={styles.row}>
          {renderKey("1")}
          {renderKey("2", "ABC")}
          {renderKey("3", "DEF")}
        </View>
        <View style={styles.row}>
          {renderKey("4", "GHI")}
          {renderKey("5", "JKL")}
          {renderKey("6", "MNO")}
        </View>
        <View style={styles.row}>
          {renderKey("7", "PQRS")}
          {renderKey("8", "TUV")}
          {renderKey("9", "WXYZ")}
        </View>
        <View style={styles.row}>
          {renderKey("+")}
          {renderKey("0")}
          <TouchableOpacity
            style={styles.key}
            onPress={() => handlePress("del")}
          >
            <Text style={styles.keyNumber}>⌫</Text>
          </TouchableOpacity>
        </View>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f6f6f6",
    paddingTop: 130,
  },

  header: {
    paddingHorizontal: 20,
    marginTop: 20,
  },

  title: {
    fontSize: 22,
    fontWeight: "600",
    marginBottom: 30,
  },

  label: {
    fontSize: 14,
    color: "#888",
    marginBottom: 8,
  },

  inputContainer: {
    flexDirection: "row",
    alignItems: "center",
    height: 50,
    borderWidth: 1,
    borderColor: "#ddd",
    borderRadius: 10,
    paddingHorizontal: 12,
    backgroundColor: "#fff",
  },

  flag: {
    fontSize: 18,
    marginRight: 8,
  },

  prefix: {
    fontSize: 16,
    marginRight: 6,
  },

  input: {
    flex: 1,
    fontSize: 16,
  },

  nextBtn: {
    position: "absolute",
    right: 20,
    top: 450, // 👉 chuẩn vị trí gần input + keypad
    width: 70,
    height: 70,
    borderRadius: 29,
    backgroundColor: "#4CAF6A",
    justifyContent: "center",
    alignItems: "center",
    elevation: 5,
  },

  arrow: {
    color: "#fff",
    fontSize: 22,
    fontWeight: "600",
  },

  keypad: {
    position: "absolute",
    bottom: 0,
    width: "100%",
    paddingHorizontal: 10,
    paddingBottom: 20,
  },

  row: {
    flexDirection: "row",
    justifyContent: "space-between",
    marginBottom: 12,
  },

  key: {
    width: "30%",
    backgroundColor: "#e9e9e9",
    borderRadius: 12,
    paddingVertical: 12,
    alignItems: "center",
    
  },

  keyNumber: {
    fontSize: 22,
    fontWeight: "600",
  },

  keyLetters: {
    fontSize: 10,
    color: "#666",
    marginTop: 2,
  },
   back: {
    marginTop: 10,
    marginBottom: 10,
    marginLeft: 10,
  },
  backIcon: {
    fontSize: 40,
    color: "#111",
  },
});