import React from 'react';
import {
  View,
  Text,
  StyleSheet,
  SafeAreaView,
  StatusBar,
  TextInput,
  Image,
  FlatList,
  TouchableOpacity,
} from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { useRouter } from 'expo-router';

export default function CategoryScreen() {
  const router = useRouter();

  const categories = [
    {
      name: 'Fresh Fruits & Vegetable',
      img: 'https://i.imgur.com/1bX5QH6.png',
      color: '#E8F5E9',
      border: '#53B175',
    },
    {
      name: 'Cooking Oil & Ghee',
      img: 'https://i.imgur.com/5u2KXGk.png',
      color: '#FFF3E0',
      border: '#F8A44C',
    },
    {
      name: 'Meat & Fish',
      img: 'https://i.imgur.com/3ZQ3Z7s.png',
      color: '#FCE4EC',
      border: '#F7A593',
    },
    {
      name: 'Bakery & Snacks',
      img: 'https://i.imgur.com/l2vZ5qH.png',
      color: '#EDE7F6',
      border: '#D3B0E0',
    },
    {
      name: 'Dairy & Eggs',
      img: 'https://i.imgur.com/7QKQZ6F.png',
      color: '#FFFDE7',
      border: '#FDE598',
    },
    {
      name: 'Beverages',
      img: 'https://i.imgur.com/UPrs1EW.png',
      color: '#E3F2FD',
      border: '#B7DFF5',
    },
  ];

  const renderItem = ({ item }) => (
    <TouchableOpacity
      style={[
        styles.card,
        { backgroundColor: item.color, borderColor: item.border },
      ]}
      onPress={() =>
  router.push({
    pathname: '/beverages',
    params: {
      name: item.name,
      img: item.img,
    },
  })
}
    >
      <Image source={{ uri: item.img }} style={styles.image} />
      <Text style={styles.name}>{item.name}</Text>
    </TouchableOpacity>
  );

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="dark-content" backgroundColor="#fff" />

      {/* Title */}
      <Text style={styles.title}>Find Products</Text>

      {/* Search */}
      <View style={styles.searchBar}>
        <Ionicons name="search-outline" size={20} color="#999" />
        <TextInput placeholder="Search Store" style={styles.input} />
      </View>

      {/* Grid */}
      <FlatList
        data={categories}
        renderItem={renderItem}
        keyExtractor={(item, index) => index.toString()}
        numColumns={2}
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ paddingBottom: 20 }}
      />
    </SafeAreaView>
  );
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    paddingHorizontal: 16,
  },

  title: {
    fontSize: 20,
    fontWeight: '600',
    textAlign: 'center',
    marginVertical: 10,
  },

  searchBar: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#F2F3F2',
    borderRadius: 12,
    paddingHorizontal: 12,
    paddingVertical: 10,
    marginBottom: 16,
  },

  input: {
    marginLeft: 8,
    flex: 1,
  },

  card: {
    flex: 1,
    margin: 8,
    borderRadius: 16,
    borderWidth: 1,
    alignItems: 'center',
    padding: 16,
  },

  image: {
    width: 80,
    height: 80,
    resizeMode: 'contain',
    marginBottom: 10,
  },

  name: {
    textAlign: 'center',
    fontWeight: '500',
  },
});