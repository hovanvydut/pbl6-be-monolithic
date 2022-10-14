insert into ExampleDb.property_group (display_name, created_at)
values 
('Các địa điểm gần đó', NOW()),
('Các tiện ích khác', NOW());

insert into ExampleDb.property (display_name, property_group_id ,created_at)
values
('Chợ, siêu thị, tạp hoá', 1, NOW()),
('Hiệu thuốc', 1, NOW()),
('Trường đại học', 1, NOW()),
('Biển', 1, NOW()),
('Cafe, trà sữa', 1, NOW()),
('Uỷ ban, công an', 1, NOW()),
('Bệnh viện', 1, NOW()),
('Công viên, bến xe', 1, NOW()),
('Sửa xe', 1, NOW()),
('Cho nam nữ ở chung', 2, NOW()),
('Cho nuôi thú cưng', 2, NOW()),
('Có sẵn nội thức', 2, NOW()),
('Có máy lạnh', 2, NOW()),
('Có máy giặt sấy', 2, NOW()),
('Có ban công', 2, NOW()),
('Có sân phơi quần áo', 2, NOW()),
('Có sân thượng', 2, NOW()),
('Có chỗ đỗ xe', 2, NOW()),
('Có camera', 2, NOW()),
('Có bảo vệ', 2, NOW()),
('Có phòng vệ sinh trong', 2, NOW()),
('Có gác lửng', 2, NOW()),
('Có nước nóng lạnh', 2, NOW()),
('Có khoá cổng riêng', 2, NOW()),
('Không quy định giờ giấc', 2, NOW()),
('Cho thuê ngắn hạn', 2, NOW());
