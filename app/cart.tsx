import React, { useContext } from 'react';
import { useRouter} from 'expo-router';

import {
  View, Text, Image, FlatList, TouchableOpacity,
  StyleSheet, SafeAreaView, Platform,
} from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { CartContext } from './CartContext';
import { router } from '@/.expo/types/router';

export default function CartScreen() {
  const { cart, increaseQty, decreaseQty, removeItem } = useContext(CartContext);
  const router = useRouter();
  const totalPrice = cart.reduce(
    (sum, item) => sum + item.price * (item.qty ?? 1), 0
  );

  const renderItem = ({ item }: any) => (
    <View style={styles.cartItem}>
      {/* Ảnh sản phẩm */}
      <Image source={item.img} style={styles.itemImg} />

      {/* Tên + weight + số lượng */}
      <View style={styles.itemInfo}>
        <Text style={styles.itemName}>{item.name}</Text>
        <Text style={styles.itemWeight}>{item.weight}, Price</Text>

        {/* Nút tăng giảm */}
        <View style={styles.qtyRow}>
          <TouchableOpacity
            style={styles.qtyBtn}
            onPress={() => decreaseQty(item.id)}
          >
            <Ionicons name="remove" size={16} color="#1A1A1A" />
          </TouchableOpacity>
          <Text style={styles.qtyText}>{item.qty ?? 1}</Text>
          <TouchableOpacity
            style={[styles.qtyBtn, styles.qtyBtnPlus]}
            onPress={() => increaseQty(item.id)}
          >
            <Ionicons name="add" size={16} color="#fff" />
          </TouchableOpacity>
        </View>
      </View>

      {/* Giá + nút xoá */}
      <View style={styles.itemRight}>
        <TouchableOpacity onPress={() => removeItem(item.id)}>
          <Ionicons name="close" size={18} color="#B3B3B3" />
        </TouchableOpacity>
        <Text style={styles.itemPrice}>${item.price.toFixed(2)}</Text>
      </View>
    </View>
  );

  return (
    <SafeAreaView style={styles.container}>

      {/* HEADER */}
      <Text style={styles.header}>My Cart</Text>

      {/* DANH SÁCH */}
      <FlatList
        data={cart}
        keyExtractor={(item) => item.id.toString()}
        renderItem={renderItem}
        showsVerticalScrollIndicator={false}
        contentContainerStyle={styles.listContent}
        ItemSeparatorComponent={() => <View style={styles.separator} />}
      />

      {/* CHECKOUT BUTTON */}
      <View style={styles.checkoutWrapper}>
        <TouchableOpacity style={styles.checkoutBtn}>
          <Text style={styles.checkoutText}>Go to Checkout</Text>
          <View style={styles.priceBadge}>
            <Text style={styles.priceBadgeText}>${totalPrice.toFixed(2)}</Text>
          </View>
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
        <TouchableOpacity style={styles.tabItem}>
          <Ionicons name="cart" size={24} color="#53B175" />
          <Text style={[styles.tabText, styles.tabTextActive]}>Cart</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.tabItem}
        onPress={() => router.push('/favourite')}>
          <Ionicons name="heart-outline" size={24} color="#7C7C7C" />
          <Text style={styles.tabText}>Favourite</Text>
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
  container: {
    flex: 1,
    backgroundColor: '#fff',
  },

  // HEADER
  header: {
    fontSize: 20,
    fontWeight: '700',
    color: '#1A1A1A',
    textAlign: 'center',
    paddingVertical: 16,
  },

  // LIST
  listContent: {
    paddingHorizontal: 20,
  },
  separator: {
    height: 1,
    backgroundColor: '#F0F0F0',
  },

  // CART ITEM
  cartItem: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 16,
    gap: 12,
  },
  itemImg: {
    width: 70,
    height: 70,
    resizeMode: 'contain',
  },
  itemInfo: {
    flex: 1,
    gap: 4,
  },
  itemName: {
    fontSize: 15,
    fontWeight: '600',
    color: '#1A1A1A',
  },
  itemWeight: {
    fontSize: 13,
    color: '#999',
  },

  // QTY ROW
  qtyRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 6,
    gap: 10,
  },
  qtyBtn: {
    width: 28,
    height: 28,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: '#D0D0D0',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#fff',
  },
  qtyBtnPlus: {
    backgroundColor: '#53B175',
    borderColor: '#53B175',
  },
  qtyText: {
    fontSize: 15,
    fontWeight: '600',
    color: '#1A1A1A',
    minWidth: 20,
    textAlign: 'center',
  },

  // ITEM RIGHT (giá + xoá)
  itemRight: {
    alignItems: 'flex-end',
    justifyContent: 'space-between',
    alignSelf: 'stretch',
    paddingVertical: 4,
  },
  itemPrice: {
    fontSize: 16,
    fontWeight: '700',
    color: '#1A1A1A',
  },

  // CHECKOUT
  checkoutWrapper: {
    paddingHorizontal: 20,
    paddingVertical: 16,
  },
  checkoutBtn: {
    backgroundColor: '#53B175',
    borderRadius: 18,
    paddingVertical: 16,
    paddingHorizontal: 24,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
  },
  checkoutText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '600',
    flex: 1,
    textAlign: 'center',
  },
  priceBadge: {
    backgroundColor: 'rgba(255,255,255,0.25)',
    borderRadius: 10,
    paddingHorizontal: 10,
    paddingVertical: 4,
  },
  priceBadgeText: {
    color: '#fff',
    fontSize: 14,
    fontWeight: '600',
  },

  // BOTTOM TAB
  bottomTabBar: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    paddingVertical: 12,
    paddingBottom: Platform.OS === 'ios' ? 20 : 12,
    backgroundColor: '#fff',
    borderTopWidth: 1,
    borderTopColor: '#F0F0F0',
  },
  tabItem: { alignItems: 'center' },
  tabText: { fontSize: 10, marginTop: 4, color: '#7C7C7C' },
  tabTextActive: { color: '#53B175' },
});