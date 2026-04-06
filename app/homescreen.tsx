import React, { useState, useEffect,useContext } from 'react';
import { useRouter, useLocalSearchParams } from 'expo-router';
import { CartContext } from './CartContext';
import {
  View,
  Text,
  Image,
  TextInput,
  ScrollView,
  TouchableOpacity,
  StyleSheet,
  SafeAreaView,
  Dimensions,
  Platform,
} from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { products } from './data';

const { width } = Dimensions.get('window');
const CARD_WIDTH = (width - 48) / 2;

export default function HomeScreen() {
  const { addToCart } = useContext(CartContext);
  const router = useRouter();
  const { category } = useLocalSearchParams();

  const [search, setSearch] = useState('');
  const [filtered, setFiltered] = useState(products);

  // 🔥 FILTER: SEARCH (realtime) + CATEGORY
  useEffect(() => {
    let result = products;

    // 🔍 Tìm kiếm theo tên (không phân biệt hoa thường, tìm gần đúng)
    if (search.trim()) {
      const searchLower = search.toLowerCase().trim();
      result = result.filter(item =>
        item.name.toLowerCase().includes(searchLower)
      );
    }

    // 🧩 Lọc theo category (nếu có)
    if (category && category !== 'All') {
      result = result.filter(item => item.category === category);
    }

    setFiltered(result);
  }, [search, category]);

  const handleSearch = (text) => {
    setSearch(text);
  };

  return (
    <SafeAreaView style={styles.container}>
      {/* SEARCH + FILTER */}
      <View style={styles.searchRow}>
        <View style={styles.searchBar}>
          <Ionicons name="search-outline" size={18} color="#999" />
          <TextInput
            placeholder="Tìm kiếm sản phẩm..."
            placeholderTextColor="#999"
            value={search}
            onChangeText={handleSearch}
            style={styles.searchInput}
            autoCapitalize="none"
            autoCorrect={false}
          />
          {search.length > 0 && (
            <TouchableOpacity onPress={() => setSearch('')}>
              <Ionicons name="close-circle" size={16} color="#999" />
            </TouchableOpacity>
          )}
        </View>

        <TouchableOpacity
          style={styles.filterBtn}
          onPress={() => router.push('/filter')}
        >
          <Ionicons name="options-outline" size={20} color="#1A1A1A" />
        </TouchableOpacity>
      </View>

      {/* PRODUCT GRID */}
      <ScrollView showsVerticalScrollIndicator={false}>
        {filtered.length === 0 ? (
          <View style={styles.emptyContainer}>
            <Ionicons name="search-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Không tìm thấy sản phẩm</Text>
            <Text style={styles.emptySubText}>
              Thử tìm với từ khóa khác nhé!
            </Text>
          </View>
        ) : (
          <View style={styles.grid}>
            {filtered.map((item) => (
              <View key={item.id} style={styles.card}>
                <Image source={item.img} style={styles.img} />
                <Text style={styles.name}>{item.name}</Text>
                <Text style={styles.weight}>{item.weight}</Text>
                <View style={styles.row}>
                  <Text style={styles.price}>${item.price}</Text>
                  <TouchableOpacity style={styles.btn}
                   onPress={() => addToCart(item)}>
                    <Ionicons name="add" size={18} color="#fff" />
                  </TouchableOpacity>
                </View>
              </View>
            ))}
          </View>
        )}
      </ScrollView>

      {/* TAB BAR */}
      <View style={styles.bottomTabBar}>
        <TouchableOpacity style={styles.tabItem}>
          <Ionicons name="home" size={24} color="#53B175" />
          <Text style={[styles.tabText, styles.tabTextActive]}>Shop</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.tabItem}>
          <Ionicons name="compass-outline" size={24} color="#7C7C7C" />
          <Text style={styles.tabText}>Explore</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.tabItem}
        onPress={() => router.push('/cart')}>
          <Ionicons name="cart-outline" size={24} color="#7C7C7C" />
          <Text style={styles.tabText}>Cart</Text>
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
    paddingTop: 30,
  },

  searchRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 16,
    paddingVertical: 10,
    gap: 10,
  },

  searchBar: {
    flex: 1,
    flexDirection: 'row',
    backgroundColor: '#F2F3F2',
    paddingHorizontal: 14,
    paddingVertical: 12,
    borderRadius: 16,
    alignItems: 'center',
  },

  searchInput: {
    flex: 1,
    marginLeft: 8,
    fontSize: 14,
  },

  filterBtn: {
    width: 44,
    height: 44,
    borderRadius: 14,
    backgroundColor: '#F2F3F2',
    alignItems: 'center',
    justifyContent: 'center',
  },

  grid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
    paddingHorizontal: 16,
  },

  card: {
    width: CARD_WIDTH,
    backgroundColor: '#fff',
    borderRadius: 16,
    padding: 12,
    marginBottom: 16,
    borderWidth: 1,
    borderColor: '#E8E8E8',
  },

  img: {
    width: '100%',
    height: 100,
    resizeMode: 'contain',
  },

  name: {
    fontWeight: '600',
    marginTop: 6,
    fontSize: 14,
  },

  weight: {
    color: '#999',
    fontSize: 12,
    marginTop: 2,
  },

  row: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 8,
    alignItems: 'center',
  },

  price: {
    fontWeight: 'bold',
    fontSize: 16,
    color: '#53B175',
  },

  btn: {
    backgroundColor: '#53B175',
    padding: 6,
    borderRadius: 10,
  },

  bottomTabBar: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    paddingVertical: 10,
    paddingBottom: Platform.OS === 'ios' ? 20 : 10,
    borderTopWidth: 1,
    borderColor: '#eee',
  },

  tabItem: {
    alignItems: 'center',
  },

  tabText: {
    fontSize: 10,
    color: '#7C7C7C',
  },

  tabTextActive: {
    color: '#53B175',
  },

  emptyContainer: {
    alignItems: 'center',
    justifyContent: 'center',
    paddingTop: 100,
  },

  emptyText: {
    fontSize: 18,
    fontWeight: '600',
    color: '#333',
    marginTop: 16,
  },

  emptySubText: {
    fontSize: 14,
    color: '#999',
    marginTop: 8,
  },
});