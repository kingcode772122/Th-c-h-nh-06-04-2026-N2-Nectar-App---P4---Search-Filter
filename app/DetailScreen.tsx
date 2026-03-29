import React from 'react';
import {
  View,
  Text,
  Image,
  ScrollView,
  TouchableOpacity,
  StyleSheet,
  SafeAreaView,
  StatusBar,
  Dimensions,
} from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { useLocalSearchParams, useRouter } from 'expo-router';

const { width } = Dimensions.get('window');

export default function DetailScreen() {
  const router = useRouter();
  const params = useLocalSearchParams();
  
  // Nhận dữ liệu từ params
  const { price, img, name } = params;
  
  // Dữ liệu mặc định nếu không có params
  const productName = name || 'Naturel Red Apple';
  const productPrice = price || '3.99';
  const productImg = img || require('../assets/Vector.png');
  
  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="dark-content" />
      
      {/* Header với nút back */}
      <View style={styles.header}>
        <TouchableOpacity onPress={() => router.back()} style={styles.backBtn}>
          <Ionicons name="arrow-back" size={24} color="#000" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>Product Details</Text>
        <View style={styles.placeholder} />
      </View>
      
      <ScrollView showsVerticalScrollIndicator={false}>
        {/* Hình ảnh sản phẩm */}
        <View style={styles.imageContainer}>
          <Image 
            source={{ uri: productImg }} 
            style={styles.productImage}
            resizeMode="contain"
          />
        </View>
        
        {/* Thông tin sản phẩm */}
        <View style={styles.infoContainer}>
          <Text style={styles.productName}>{productName}</Text>
          <Text style={styles.productWeight}>1kg, Price</Text>
          
          <Text style={styles.productPrice}>${productPrice}</Text>
        </View>
        
        {/* Đường kẻ ngang */}
        <View style={styles.divider} />
        
        {/* Product Detail */}
        <View style={styles.sectionContainer}>
          <Text style={styles.sectionTitle}>Product Detail</Text>
          <Text style={styles.sectionContent}>
            Apples Are Nutritious. Apples May Be Good For Weight Loss. 
            Apples May Be Good For Your Heart. As Part Of A Healthful And Varied Diet.
          </Text>
        </View>
        
        {/* Đường kẻ ngang */}
        <View style={styles.divider} />
        
        {/* Nutrients */}
        <View style={styles.sectionContainer}>
          <Text style={styles.sectionTitle}>Nutrients</Text>
          <Text style={styles.nutrientValue}>100g</Text>
          
          <View style={styles.nutrientRow}>
            <Text style={styles.nutrientLabel}>Energy</Text>
            <Text style={styles.nutrientAmount}>52 kcal</Text>
          </View>
          
          <View style={styles.nutrientRow}>
            <Text style={styles.nutrientLabel}>Protein</Text>
            <Text style={styles.nutrientAmount}>0.3g</Text>
          </View>
          
          <View style={styles.nutrientRow}>
            <Text style={styles.nutrientLabel}>Fat</Text>
            <Text style={styles.nutrientAmount}>0.2g</Text>
          </View>
          
          <View style={styles.nutrientRow}>
            <Text style={styles.nutrientLabel}>Carbohydrates</Text>
            <Text style={styles.nutrientAmount}>14g</Text>
          </View>
        </View>
        
        {/* Đường kẻ ngang */}
        <View style={styles.divider} />
        
        {/* Review */}
        <View style={styles.sectionContainer}>
          <Text style={styles.sectionTitle}>Review</Text>
          <View style={styles.ratingContainer}>
            <View style={styles.stars}>
              <Ionicons name="star" size={20} color="#FFB800" />
              <Ionicons name="star" size={20} color="#FFB800" />
              <Ionicons name="star" size={20} color="#FFB800" />
              <Ionicons name="star" size={20} color="#FFB800" />
              <Ionicons name="star" size={20} color="#FFB800" />
            </View>
            <Text style={styles.reviewText}>(4.8)</Text>
          </View>
          <Text style={styles.reviewComment}>
            "Fresh and delicious apples, perfect for snacking!"
          </Text>
          <Text style={styles.reviewer}>- John Doe</Text>
        </View>
        
        <View style={{ height: 100 }} />
      </ScrollView>
      
      {/* Add to Basket Button */}
      <View style={styles.bottomContainer}>
        <TouchableOpacity 
          style={styles.addToBasketBtn}
          onPress={() => {
            // Xử lý thêm vào giỏ hàng
            console.log('Added to basket:', productName, productPrice);
            // Có thể hiển thị thông báo hoặc điều hướng
          }}
        >
          <Text style={styles.addToBasketText}>Add To Basket</Text>
        </TouchableOpacity>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FFFFFF',
  },
  
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingHorizontal: 16,
    paddingVertical: 12,
    backgroundColor: '#FFFFFF',
    borderBottomWidth: 1,
    borderBottomColor: '#F0F0F0',
  },
  
  backBtn: {
    padding: 4,
  },
  
  headerTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#000000',
  },
  
  placeholder: {
    width: 32,
  },
  
  imageContainer: {
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 20,
    backgroundColor: '#F9F9F9',
    marginHorizontal: 16,
    marginTop: 16,
    borderRadius: 20,
  },
  
  productImage: {
    width: width - 80,
    height: 250,
  },
  
  infoContainer: {
    paddingHorizontal: 20,
    paddingTop: 20,
    paddingBottom: 16,
  },
  
  productName: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#000000',
    marginBottom: 8,
  },
  
  productWeight: {
    fontSize: 14,
    color: '#999999',
    marginBottom: 12,
  },
  
  productPrice: {
    fontSize: 28,
    fontWeight: 'bold',
    color: '#53B175',
  },
  
  divider: {
    height: 1,
    backgroundColor: '#F0F0F0',
    marginHorizontal: 20,
    marginVertical: 16,
  },
  
  sectionContainer: {
    paddingHorizontal: 20,
    paddingVertical: 8,
  },
  
  sectionTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#000000',
    marginBottom: 12,
  },
  
  sectionContent: {
    fontSize: 14,
    lineHeight: 22,
    color: '#666666',
    textAlign: 'justify',
  },
  
  nutrientValue: {
    fontSize: 16,
    fontWeight: '500',
    color: '#53B175',
    marginBottom: 16,
  },
  
  nutrientRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#F5F5F5',
  },
  
  nutrientLabel: {
    fontSize: 15,
    color: '#333333',
  },
  
  nutrientAmount: {
    fontSize: 15,
    fontWeight: '500',
    color: '#000000',
  },
  
  ratingContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 12,
  },
  
  stars: {
    flexDirection: 'row',
    marginRight: 8,
  },
  
  reviewText: {
    fontSize: 14,
    color: '#FFB800',
    fontWeight: '500',
  },
  
  reviewComment: {
    fontSize: 14,
    lineHeight: 20,
    color: '#666666',
    marginBottom: 8,
    fontStyle: 'italic',
  },
  
  reviewer: {
    fontSize: 13,
    color: '#999999',
  },
  
  bottomContainer: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    backgroundColor: '#FFFFFF',
    paddingHorizontal: 20,
    paddingVertical: 16,
    borderTopWidth: 1,
    borderTopColor: '#F0F0F0',
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: -2,
    },
    shadowOpacity: 0.05,
    shadowRadius: 3,
    elevation: 5,
  },
  
  addToBasketBtn: {
    backgroundColor: '#53B175',
    borderRadius: 20,
    paddingVertical: 16,
    alignItems: 'center',
    justifyContent: 'center',
  },
  
  addToBasketText: {
    color: '#FFFFFF',
    fontSize: 18,
    fontWeight: '600',
  },
});