import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import { useRouter } from 'expo-router';
import { useState } from 'react';

export default function FilterScreen() {
  const router = useRouter();

  // State tạm thời cho categories (thay đổi UI nhưng chưa áp dụng)
  const [tempCategories, setTempCategories] = useState([
    { name: 'Eggs', checked: true },
    { name: 'Noodles & Pasta', checked: false },
    { name: 'Chips & Crisps', checked: false },
    { name: 'Fast Food', checked: false },
  ]);

  // Brand chỉ để hiển thị, không có state xử lý
  const brands = [
    'Individual Collection',
    'Cocola',
    'Ifad',
    'Kazi Farmas',
  ];

  // Hàm xử lý khi bấm chọn category (chỉ cập nhật UI tạm thời)
  const handleCategoryPress = (categoryName: string) => {
    setTempCategories(prev =>
      prev.map(item =>
        item.name === categoryName
          ? { ...item, checked: true }
          : { ...item, checked: false }
      )
    );
  };

  // Hàm xử lý khi bấm Apply Filter
  const handleApplyFilter = () => {
    // Tìm category đang được checked
    const selectedCategory = tempCategories.find(item => item.checked === true);
    
    if (selectedCategory) {
      // Chuyển sang màn home và gửi category đã chọn
      router.push({
        pathname: '/homescreen',
        params: { category: selectedCategory.name },
      });
    } else {
      // Nếu không có category nào được chọn, có thể gửi giá trị mặc định hoặc thông báo
      router.push({
        pathname: '/homescreen',
        params: { category: 'All' },
      });
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Filters</Text>

      {/* Categories Section */}
      <Text style={styles.sectionTitle}>Categories</Text>
      {tempCategories.map((item) => (
        <TouchableOpacity
          key={item.name}
          style={styles.item}
          onPress={() => handleCategoryPress(item.name)}
          activeOpacity={0.7}
        >
          <View style={styles.checkbox}>
            {item.checked && <View style={styles.checkboxInner} />}
          </View>
          <Text style={styles.itemText}>{item.name}</Text>
        </TouchableOpacity>
      ))}

      {/* Brand Section - CHỈ HIỂN THỊ, không xử lý */}
      <Text style={styles.sectionTitle}>Brand</Text>
      {brands.map((item) => (
        <View key={item} style={styles.item}>
          <View style={styles.checkbox} />
          <Text style={styles.itemText}>{item}</Text>
        </View>
      ))}

      {/* Apply Filter Button */}
      <TouchableOpacity style={styles.applyButton} onPress={handleApplyFilter}>
        <Text style={styles.applyButtonText}>Apply Filter</Text>
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    paddingTop: 60,
    backgroundColor: '#fff',
  },
  title: {
    fontSize: 28,
    fontWeight: 'bold',
    marginBottom: 24,
    color: '#000',
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: '600',
    marginBottom: 12,
    marginTop: 8,
    color: '#333',
  },
  item: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 14,
    borderBottomWidth: 1,
    borderBottomColor: '#F0F0F0',
  },
  checkbox: {
    width: 22,
    height: 22,
    borderRadius: 6,
    borderWidth: 2,
    borderColor: '#4CAF50',
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: 12,
    backgroundColor: '#fff',
  },
  checkboxInner: {
    width: 12,
    height: 12,
    borderRadius: 3,
    backgroundColor: '#4CAF50',
  },
  itemText: {
    fontSize: 16,
    color: '#222',
  },
  applyButton: {
    backgroundColor: '#4CAF50',
    paddingVertical: 14,
    borderRadius: 8,
    marginTop: 24,
    alignItems: 'center',
  },
  applyButtonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: '600',
  },
});