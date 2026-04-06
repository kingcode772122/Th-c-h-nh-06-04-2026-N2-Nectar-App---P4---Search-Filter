import React, { useContext } from 'react';
import {
  View, Text, Image, FlatList, TouchableOpacity,
  StyleSheet, SafeAreaView, Platform,
} from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { CartContext } from './CartContext';
import { useRouter} from 'expo-router';

// Data yêu thích mẫu - bạn thay bằng FavouriteContext nếu có
const favourites = [
  { id: 1, name: 'Sprite Can', weight: '325ml', price: 1.50, img: require('../assets/kq1.jpg') },
  { id: 2, name: 'Diet Coke', weight: '355ml', price: 1.99, img: require('../assets/kq3.jpg') },
  { id: 3, name: 'Apple & Grape Juice', weight: '2L', price: 15.50, img: require('../assets/kq2jpg.jpg') },
  { id: 4, name: 'Coca Cola Can', weight: '325ml', price: 4.99, img: require('../assets/kq4.jpg') },
  { id: 5, name: 'Pepsi Can', weight: '330ml', price: 4.99, img: require('../assets/mi.png') },
];

export default function FavouriteScreen() {
  const { addToCart } = useContext(CartContext);
 const router = useRouter();
  const addAllToCart = () => {
    favourites.forEach(item => addToCart(item));
  };

  const renderItem = ({ item }: any) => (
    <TouchableOpacity style={styles.item} activeOpacity={0.7}>
      <Image source={item.img} style={styles.itemImg} />
      <View style={styles.itemInfo}>
        <Text style={styles.itemName}>{item.name}</Text>
        <Text style={styles.itemWeight}>{item.weight}, Price</Text>
      </View>
      <Text style={styles.itemPrice}>${item.price.toFixed(2)}</Text>
      <Ionicons name="chevron-forward" size={18} color="#B3B3B3" />
    </TouchableOpacity>
  );

  return (
    <SafeAreaView style={styles.container}>

      {/* HEADER */}
      <Text style={styles.header}>Favouruite</Text>

      {/* LIST */}
      <FlatList
        data={favourites}
        keyExtractor={(item) => item.id.toString()}
        renderItem={renderItem}
        showsVerticalScrollIndicator={false}
        contentContainerStyle={styles.listContent}
        ItemSeparatorComponent={() => <View style={styles.separator} />}
      />

      {/* ADD ALL TO CART */}
      <View style={styles.btnWrapper}>
        <TouchableOpacity style={styles.addAllBtn} onPress={addAllToCart}>
          <Text style={styles.addAllText}>Add All To Cart</Text>
        </TouchableOpacity>
      </View>

      {/* BOTTOM TAB BAR */}
      <View style={styles.bottomTabBar}>
        <TouchableOpacity style={styles.tabItem}
        onPress={() => router.push('/homescreen')}>
          <Ionicons name="home-outline" size={24} color="#7C7C7C" />
          <Text style={styles.tabText}>Shop</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.tabItem}>
          <Ionicons name="search-outline" size={24} color="#7C7C7C" />
          <Text style={styles.tabText}>Explore</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.tabItem}
        onPress={() => router.push('/cart')}>
          <Ionicons name="cart-outline" size={24} color="#7C7C7C" />
          <Text style={styles.tabText}>Cart</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.tabItem}>
          <Ionicons name="heart" size={24} color="#53B175" />
          <Text style={[styles.tabText, styles.tabTextActive]}>Favourite</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.tabItem}>
          <Ionicons name="person-outline" size={24} color="#7C7C7C" />
          <Text style={styles.tabText}>Account</Text>
        </TouchableOpacity>
      </View>

    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#fff' },

  header: {
    fontSize: 20, fontWeight: '700', color: '#1A1A1A',
    textAlign: 'center', paddingVertical: 16,
  },

  listContent: { paddingHorizontal: 20 },

  separator: { height: 1, backgroundColor: '#F0F0F0' },

  item: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 16,
    gap: 12,
  },
  itemImg: {
    width: 60, height: 60, resizeMode: 'contain',
  },
  itemInfo: { flex: 1 },
  itemName: { fontSize: 15, fontWeight: '600', color: '#1A1A1A', marginBottom: 4 },
  itemWeight: { fontSize: 13, color: '#999' },
  itemPrice: { fontSize: 15, fontWeight: '700', color: '#1A1A1A', marginRight: 6 },

  btnWrapper: {
    paddingHorizontal: 20,
    paddingVertical: 16,
  },
  addAllBtn: {
    backgroundColor: '#53B175',
    borderRadius: 18,
    paddingVertical: 16,
    alignItems: 'center',
  },
  addAllText: { color: '#fff', fontSize: 16, fontWeight: '600' },

  bottomTabBar: {
    flexDirection: 'row', justifyContent: 'space-around', alignItems: 'center',
    paddingVertical: 12,
    paddingBottom: Platform.OS === 'ios' ? 20 : 12,
    backgroundColor: '#fff', borderTopWidth: 1, borderTopColor: '#F0F0F0',
  },
  tabItem: { alignItems: 'center' },
  tabText: { fontSize: 10, marginTop: 4, color: '#7C7C7C' },
  tabTextActive: { color: '#53B175' },
});