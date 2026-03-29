import React from 'react';
import {
  View,
  Text,
  Image,
  TextInput,
  ScrollView,
  TouchableOpacity,
  StyleSheet,
  SafeAreaView,
  StatusBar,
  Dimensions,
} from 'react-native';
import { Ionicons, Feather } from '@expo/vector-icons';
import { useRouter } from 'expo-router';

const { width } = Dimensions.get('window');
const CARD_WIDTH = (width - 56) / 2;

// DATA phải được định nghĩa trước khi dùng
const data = [
  {
    name: 'Organic Bananas',
    weight: '7pcs, Priceg',
    price: '4.99',
    img: require('../assets/Vector.png'),
  },
  {
    name: 'Red Apple',
    weight: '1kg, Priceg',
    price: '4.99',
    img: require('../assets/Vector.png'),
  },
  {
    name: 'Organic Bananas',
    weight: '7pcs, Priceg',
    price: '4.99',
    img: require('../assets/Vector.png'),
  },
  {
    name: 'Red Apple',
    weight: '1kg, Priceg',
    price: '4.99',
    img: require('../assets/Vector.png'),
  },
];

const categories = [
  {
    name: 'Vegetables',
    img: require('../assets/pngfuel5.png'),
    bg: '#FFFFFF',
  },
  {
    name: 'Fruits',
    img: require('../assets/pngfuel5.png'),
    bg: '#FFFFFF',
  },
  {
    name: 'Meat',
    img: require('../assets/pngfuel5.png'),
    bg: '#FFFFFF',
  },
];

export default function App() {
  const router = useRouter();

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="dark-content" />

      {/* HEADER */}
      <View style={styles.header}>
        <Text style={styles.logo}>🥕</Text>

        <View style={styles.locationContainer}>
          <Ionicons name="location-sharp" size={14} color="#666" />
          <Text style={styles.locationText}>Dhaka, Banassre</Text>
        </View>
      </View>

      <ScrollView showsVerticalScrollIndicator={false}>
        {/* SEARCH */}
        <View style={styles.searchContainer}>
          <View style={styles.searchBar}>
            <Ionicons name="search-outline" size={18} color="#999" />
            <TextInput placeholder="Search Store" style={styles.searchInput} />
          </View>
        </View>

        {/* BANNER */}
        <View style={styles.banner}>
          <Image
            source={require("../assets/banner.png")}
            style={styles.bannerImg}
            resizeMode="cover"
          />
        </View>

        {/* SECTION */}
        <Section title="Exclusive Offer" />

        <ScrollView horizontal showsHorizontalScrollIndicator={false}>
          <View style={styles.row}>
            {data.map((item, i) => (
              <Card key={i} item={item} />
            ))}
          </View>
        </ScrollView>

        {/* BEST SELLING */}
        <Section title="Best Selling" />

        <View style={styles.grid}>
          {data.map((item, i) => (
            <GridCard key={i} item={item} router={router} />
          ))}
        </View>

        <View style={{ height: 90 }} />
        
        {/* "Groceries"*/}
        <Section title="Groceries" />
        <ScrollView horizontal showsHorizontalScrollIndicator={false}>
          <View style={styles.categoryRow}>
            {categories.map((item, index) => (
              <View key={index} style={[styles.categoryCard, { backgroundColor: item.bg }]}>
                <Image source={item.img} style={styles.categoryImg} />
                <Text style={styles.categoryText}>{item.name}</Text>
              </View>
            ))}
          </View>
        </ScrollView>
        
        <ScrollView horizontal showsHorizontalScrollIndicator={false}>
          <View style={styles.row}>
            {data.map((item, i) => (
              <Card key={i} item={item} />
            ))}
          </View>
        </ScrollView>
      </ScrollView>

      {/* TAB BAR */}
      <View style={styles.tabBar}>
        <Tab icon="home" label="Shop" active />
        <Tab icon="compass-outline" label="Explore" onPress={() => router.push('/explore')}/>
        <Tab icon="cart-outline" label="Cart" />
        <Tab icon="heart-outline" label="Favourite" />
        <Tab icon="person-outline" label="Account" />
      </View>
    </SafeAreaView>
  );
}

/* COMPONENTS */

const Section = ({ title }) => (
  <View style={styles.section}>
    <Text style={styles.sectionTitle}>{title}</Text>
    <Text style={styles.seeAll}>See all</Text>
  </View>
);

const Card = ({ item }) => (
  <View style={styles.card}>
    <Image source={item.img } style={styles.cardImg} />
    <Text style={styles.cardName}>{item.name}</Text>
    <Text style={styles.cardWeight}>{item.weight}</Text>

    <View style={styles.priceRow}>
      <Text style={styles.price}>${item.price}</Text>
      <TouchableOpacity style={styles.addBtn}>
        <Ionicons name="add" color="#fff" size={16} />
      </TouchableOpacity>
    </View>
  </View>
);

// Sửa GridCard - nhận router từ props
const GridCard = ({ item, router }) => (
  <View style={styles.gridCard}>
    <Image source={item.img} style={styles.gridImg} />
    <Text style={styles.cardName}>{item.name}</Text>
    <Text style={styles.cardWeight}>{item.weight}</Text>

    <View style={styles.priceRow}>
      <Text style={styles.price}>${item.price}</Text>
      <TouchableOpacity
        style={styles.addBtn}
        onPress={() => {
          router.push({
            pathname: '/DetailScreen', // Chú ý: DetailScreen (S hoa)
            params: { 
              price: item.price,
              img: item.img,
              name: item.name
            }
          });
        }}
      >
        <Ionicons name="add" color="#fff" size={20} />
      </TouchableOpacity>
    </View>
  </View>
);

const Tab = ({ icon, label, active, onPress }) => (
  <TouchableOpacity style={styles.tabItem} onPress={onPress}>
    <Ionicons name={icon} size={22} color={active ? '#53B175' : '#999'} />
    <Text style={{ fontSize: 11, color: active ? '#53B175' : '#999' }}>
      {label}
    </Text>
  </TouchableOpacity>
);
/* STYLES */
const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#fff' },

  /* HEADER */
  header: {
    alignItems: 'center',
    paddingVertical: 10,
  },
  logo: { fontSize: 22 },

  locationContainer: {
    flexDirection: 'row',
    backgroundColor: '#F2F3F2',
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 20,
    alignItems: 'center',
    marginTop: 4,
  },
  locationText: {
    marginLeft: 4,
    fontSize: 13,
    fontWeight: '600',
  },

  /* SEARCH */
  searchContainer: {
    flexDirection: 'row',
    paddingHorizontal: 16,
    marginTop: 10,
  },
  searchBar: {
    flex: 1,
    backgroundColor: '#F2F3F2',
    borderRadius: 15,
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 12,
  },
  searchInput: { marginLeft: 6 },

  filterBtn: {
    marginLeft: 10,
    backgroundColor: '#F2F3F2',
    padding: 10,
    borderRadius: 12,
  },

  /* BANNER */
  banner: {
    marginHorizontal: 16,
    marginTop: 10,
    borderRadius: 20,
    overflow: 'hidden',
  },
  bannerTitle: { fontWeight: 'bold', fontSize: 16 },
  bannerSub: { color: '#53B175', marginTop: 4 },
  bannerImg: {
    width: '100%',
    height: 120,
  },

  /* SECTION */
  section: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingHorizontal: 16,
    marginTop: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
  },
  seeAll: {
    color: '#53B175',
  },

  /* CARD */
  row: { flexDirection: 'row', paddingLeft: 16 },

  card: {
    width: 140,
    backgroundColor: '#F9F9F9',
    borderRadius: 16,
    padding: 10,
    marginRight: 12,
  },
  cardImg: { width: '100%', height: 90 },
  cardName: { fontWeight: '600', marginTop: 6 },
  cardWeight: { fontSize: 11, color: '#999' },

  priceRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 6,
    alignItems: 'center',
  },
  price: { fontWeight: 'bold', color: '#53B175' },

  addBtn: {
    backgroundColor: '#53B175',
    padding: 6,
    borderRadius: 10,
  },

  /* GRID */
  grid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    paddingHorizontal: 16,
    justifyContent: 'space-between',
  },
  gridCard: {
    width: CARD_WIDTH,
    backgroundColor: '#F9F9F9',
    borderRadius: 16,
    padding: 10,
    marginTop: 10,
  },
  gridImg: { width: '100%', height: 100 },

  /* TAB */
  tabBar: {
    position: 'absolute',
    bottom: 0,
    flexDirection: 'row',
    backgroundColor: '#fff',
    width: '100%',
    paddingVertical: 10,
    borderTopWidth: 1,
    borderColor: '#eee',
  },
  tabItem: {
    flex: 1,
    alignItems: 'center',
  },

  categoryRow: {
    flexDirection: 'row',
    paddingLeft: 16,
    marginTop: 10,
  },

  categoryCard: {
    width: 150,
    height: 80,
    borderRadius: 16,
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 12,
    marginRight: 10,
  },

  categoryImg: {
    width: 150,
    height: 100,
    borderRadius: 10,
  },

  categoryText: {
    marginLeft: 10,
    fontWeight: '600',
    fontSize: 14,
  },
});