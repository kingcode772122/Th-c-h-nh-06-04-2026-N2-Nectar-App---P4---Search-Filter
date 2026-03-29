import React from 'react';
import {
  SafeAreaView,
  View,
  Text,
  Image,
  TouchableOpacity,
  StyleSheet,
  StatusBar,
  FlatList,
} from 'react-native';
import { Ionicons } from '@expo/vector-icons';

const beverages = [
  {
    id: 1,
    name: 'Diet Coke',
    size: '355ml',
    price: '$1.99',
    image: 'https://i.imgur.com/8Km9tLL.png',
  },
  {
    id: 2,
    name: 'Sprite Can',
    size: '325ml',
    price: '$1.50',
    image: 'https://i.imgur.com/2nCt3Sbl.png',
  },
  {
    id: 3,
    name: 'Apple & Grape Juice',
    size: '2L',
    price: '$15.99',
    image: 'https://i.imgur.com/7AqZ8vY.png',
  },
  {
    id: 4,
    name: 'Orange Juice',
    size: '2L',
    price: '$15.99',
    image: 'https://i.imgur.com/Y3G9V7h.png',
  },
  {
    id: 5,
    name: 'Coca Cola Can',
    size: '325ml',
    price: '$4.99',
    image: 'https://i.imgur.com/9XnKx1v.png',
  },
  {
    id: 6,
    name: 'Pepsi Can',
    size: '330ml',
    price: '$4.99',
    image: 'https://i.imgur.com/pepsi.png',
  },
];

export default function BeverageScreen() {
  const renderItem = ({ item }) => (
    <View style={styles.card}>
      <Image source={{ uri: item.image }} style={styles.image} />

      <Text style={styles.name}>{item.name}</Text>
      <Text style={styles.size}>{item.size}, Price</Text>

      <View style={styles.bottomRow}>
        <Text style={styles.price}>{item.price}</Text>

        <TouchableOpacity style={styles.addBtn}>
          <Ionicons name="add" size={20} color="#fff" />
        </TouchableOpacity>
      </View>
    </View>
  );

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="dark-content" />

      {/* HEADER */}
      <View style={styles.header}>
        <Ionicons name="arrow-back" size={22} />
        <Text style={styles.title}>Beverages</Text>
        <Ionicons name="options-outline" size={22} />
      </View>

      {/* GRID */}
      <FlatList
        data={beverages}
        renderItem={renderItem}
        keyExtractor={(item) => item.id.toString()}
        numColumns={2}
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ padding: 10 }}
      />
    </SafeAreaView>
  );
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F2F3F2',
  },

  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingHorizontal: 16,
    paddingVertical: 12,
    backgroundColor: '#fff',
  },

  title: {
    fontSize: 18,
    fontWeight: '600',
  },

  card: {
    flex: 1,
    backgroundColor: '#fff',
    margin: 8,
    borderRadius: 18,
    padding: 12,
    borderWidth: 1,
    borderColor: '#E2E2E2',
  },

  image: {
    width: '100%',
    height: 100,
    resizeMode: 'contain',
    marginBottom: 10,
  },

  name: {
    fontSize: 14,
    fontWeight: '600',
    marginBottom: 2,
  },

  size: {
    fontSize: 12,
    color: '#7C7C7C',
    marginBottom: 10,
  },

  bottomRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },

  price: {
    fontSize: 16,
    fontWeight: '600',
  },

  addBtn: {
    width: 36,
    height: 36,
    borderRadius: 12,
    backgroundColor: '#53B175',
    justifyContent: 'center',
    alignItems: 'center',
  },
});