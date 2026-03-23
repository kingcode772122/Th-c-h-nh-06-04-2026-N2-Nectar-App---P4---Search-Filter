import React, { useState } from "react";
import {
  View,
  Text,
  TouchableOpacity,
  StyleSheet,
  SafeAreaView,
} from "react-native";
import { useRouter } from "expo-router";
export default function PhoneScreen() {
  const router = useRouter();
  const [code, setCode] = useState("");

  const handlePress = (value) => {
    if (value === "del") {
      setCode(code.slice(0, -1));
    } else if (code.length < 4) {
      setCode(code + value);
    }
  };

  const renderKey = (num, letters = "") => (
    <TouchableOpacity
      style={styles.key}
      onPress={() => handlePress(num)}
      activeOpacity={0.6}
    >
      <Text style={styles.keyNumber}>{num}</Text>
      {letters ? <Text style={styles.keyLetters}>{letters}</Text> : null}
    </TouchableOpacity>
  );

  const renderDots = () => {
    return [0, 1, 2, 3].map((i) => (
      <View
        key={i}
        style={[
          styles.dot,
          { opacity: i < code.length ? 1 : 0.2 },
        ]}
      />
    ));
  };

  return (
    <SafeAreaView style={styles.container}>
        <TouchableOpacity style={styles.back}
              onPress={() => router.replace("/number")}>
                <Text style={styles.backIcon}>‹</Text>
              </TouchableOpacity>
      {/* HEADER */}
      <View style={styles.header}>
        <Text style={styles.title}>Enter your 4-digit code</Text>

        <Text style={styles.label}>Code</Text>
<View style={styles.codeBox}>{renderDots()}</View>
        <View style={styles.codeWrapper}>
          <View style={styles.dash} />
          
        </View>

        <Text style={styles.resend}>Resend Code</Text>
      </View>

      {/* NEXT BUTTON */}
      <TouchableOpacity style={styles.nextBtn}
      onPress={() => router.replace("/location")}>
        <Text style={styles.arrow}>›</Text>
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
          <Text style={styles.symbols}>+*#</Text>
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
    backgroundColor: "#f2f2f2",
    marginTop: 25,
  },

  header: {
    paddingHorizontal: 24,
    marginTop: 50,
  },

  title: {
    fontSize: 24,
    fontWeight: "600",
    color: "#111",
    marginBottom: 30,
  },

  label: {
    fontSize: 14,
    color: "#9a9a9a",
    marginBottom: 8,
  },

  codeWrapper: {
    marginBottom: 25,
    marginTop:20
  },

  dash: {
    height: 1,
    backgroundColor: "#ccc",
    marginBottom: 12,
    width: "100%",
  },

  codeBox: {
    flexDirection: "row",
    gap: 12,
  },

  dot: {
    width: 10,
    height: 10,
    borderRadius: 5,
    backgroundColor: "#111",
  },

  resend: {
    color: "#4CAF6A",
    fontSize: 14,
    marginTop: 200,
  },

  nextBtn: {
    position: "absolute",
    right: 20,
    top: "48%",
    width: 64,
    height: 64,
    borderRadius: 32,
    backgroundColor: "#4CAF6A",
    justifyContent: "center",
    alignItems: "center",
  },

  arrow: {
    color: "#fff",
    fontSize: 32,
    fontWeight: "500",
  },

  keypad: {
    position: "absolute",
    bottom: 10,
    width: "100%",
    paddingHorizontal: 20,
  },

  row: {
    flexDirection: "row",
    justifyContent: "space-between",
    marginBottom: 12,
  },

  key: {
    width: 90,
    height: 60,
    borderRadius: 12,
    backgroundColor: "#e5e5e5",
    justifyContent: "center",
    alignItems: "center",
  },

  keyNumber: {
    fontSize: 26,
    fontWeight: "500",
    color: "#111",
  },

  keyLetters: {
    fontSize: 10,
    color: "#666",
    marginTop: 2,
  },

  symbols: {
    width: 90,
    textAlign: "center",
    fontSize: 16,
    color: "#333",
    alignSelf: "center",
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