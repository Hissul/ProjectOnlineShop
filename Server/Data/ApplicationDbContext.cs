using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Data.Entities;

namespace Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductInfo> ProductInfos { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) {
    }

    protected override void OnModelCreating (ModelBuilder builder) {
        base.OnModelCreating (builder);
        
        byte[] image1 = File.ReadAllBytes("E:\\ForCourse\\starry_night.jpg");
        byte[] image2 = File.ReadAllBytes("E:\\ForCourse\\self_portrait.jpg");
        byte[] image3 = File.ReadAllBytes("E:\\ForCourse\\mona_lisa.jpg");
        byte[] image4 = File.ReadAllBytes("E:\\ForCourse\\last_supper.jpg");
        byte[] image5 = File.ReadAllBytes("E:\\ForCourse\\girl_with_a_pearl_earring.jpg");
        byte[] image6 = File.ReadAllBytes("E:\\ForCourse\\the_scream.jpg");
        byte[] image7 = File.ReadAllBytes("E:\\ForCourse\\green_meadow.jpg");
        byte[] image8 = File.ReadAllBytes("E:\\ForCourse\\impression_sunrise.jpg");
        byte[] image9 = File.ReadAllBytes("E:\\ForCourse\\birth_of_venus.jpg");
        byte[] image10 = File.ReadAllBytes("E:\\ForCourse\\night_watch.jpg");
        byte[] image11 = File.ReadAllBytes("E:\\ForCourse\\three_musicians.jpg");
        byte[] image12 = File.ReadAllBytes("E:\\ForCourse\\guernica.jpg");
        byte[] image13 = File.ReadAllBytes("E:\\ForCourse\\cosmic_look.jpg");
        byte[] image14 = File.ReadAllBytes("E:\\ForCourse\\luncheon_on_the_grass.jpg");
        byte[] image15 = File.ReadAllBytes("E:\\ForCourse\\morning_in_monte_carlo.jpg");
        byte[] image16 = File.ReadAllBytes("E:\\ForCourse\\great_wave.jpg");
        byte[] image17 = File.ReadAllBytes("E:\\ForCourse\\violinist_on_the_roof.jpg");
        byte[] image18 = File.ReadAllBytes("E:\\ForCourse\\noise_of_the_sea.jpg");
        byte[] image19 = File.ReadAllBytes("E:\\ForCourse\\sunrise_on_the_square.jpg");
        byte[] image20 = File.ReadAllBytes("E:\\ForCourse\\kids_on_the_beach.jpg");

        builder.Entity<Product> ().HasData (
            new Product {
                Id = 1,
                Name = "Звездная ночь",
                Description = "Картина Винсента Ван Гога, изображающая вид из окна его комнаты.",
                Price = 1500.00m,
                StockQuantity = 5,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image1
            },
            new Product {
                Id = 2,
                Name = "Автопортрет",
                Description = "Автопортрет Ван Гога, написанный в 1889 году.",
                Price = 1200.00m,
                StockQuantity = 4,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image2
            },
            new Product {
                Id = 3,
                Name = "Мона Лиза",
                Description = "Картина Леонардо да Винчи, изображающая женщину с загадочной улыбкой.",
                Price = 2500.00m,
                StockQuantity = 3,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image3
            },
            new Product {
                Id = 4,
                Name = "Тайная вечеря",
                Description = "Картина Леонардо да Винчи, изображающая момент, когда Иисус сообщил своим ученикам, что один из них его предаст.",
                Price = 3000.00m,
                StockQuantity = 2,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image4
            },
            new Product {
                Id = 5,
                Name = "Девушка с жемчужной сережкой",
                Description = "Картина Яна Вермеера, изображающая девушку в экзотическом головном уборе.",
                Price = 1800.00m,
                StockQuantity = 5,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image5
            },
            new Product {
                Id = 6,
                Name = "Крик",
                Description = "Картина Эдварда Мунка, изображающая фигуру, кричащую на фоне фьорда в Норвегии.",
                Price = 2000.00m,
                StockQuantity = 6,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image6
            },
            new Product {
                Id = 7,
                Name = "Зеленая поляна",
                Description = "Картина Клода Моне, изображающая цветущий сад и зеленые луга.",
                Price = 2200.00m,
                StockQuantity = 4,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image7
            },
            new Product {
                Id = 8,
                Name = "Импрессия. Восходящее солнце",
                Description = "Картина Клода Моне, давшая название художественному направлению импрессионизм.",
                Price = 2600.00m,
                StockQuantity = 5,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image8
            },
            new Product {
                Id = 9,
                Name = "Рождение Венеры",
                Description = "Картина Сандро Боттичелли, изображающая миф о рождении богини Венеры.",
                Price = 3000.00m,
                StockQuantity = 2,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image9
            },
            new Product {
                Id = 10,
                Name = "Ночной дозор",
                Description = "Картина Рембрандта, изображающая вооруженную охрану в ночное время.",
                Price = 2800.00m,
                StockQuantity = 3,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image10
            },
            new Product {
                Id = 11,
                Name = "Три музыканта",
                Description = "Картина Пабло Пикассо, выполненная в стиле кубизма.",
                Price = 3500.00m,
                StockQuantity = 4,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image11
            },
            new Product {
                Id = 12,
                Name = "Герника",
                Description = "Картина Пабло Пикассо, посвященная трагедии войны и разрушениям.",
                Price = 5000.00m,
                StockQuantity = 3,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image12
            },
            new Product {
                Id = 13,
                Name = "Космический взгляд",
                Description = "Абстрактная картина Джексона Поллока, изображающая вселенную в динамике.",
                Price = 2400.00m,
                StockQuantity = 5,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image13
            },
            new Product {
                Id = 14,
                Name = "Завтрак на траве",
                Description = "Картина Эдуарда Мане, изображающая пикник на траве с участием обнаженной женщины.",
                Price = 2200.00m,
                StockQuantity = 4,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image14
            },
            new Product {
                Id = 15,
                Name = "Утро в Монте Карло",
                Description = "Картина Жоржа Сёра, выполненная в технике пуантилизма.",
                Price = 2700.00m,
                StockQuantity = 6,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image15
            },
            new Product {
                Id = 16,
                Name = "Большая волна в Канагаве",
                Description = "Картина Хокусая, изображающая гигантскую волну, грозящую перевернуть лодки.",
                Price = 1600.00m,
                StockQuantity = 7,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image16
            },
            new Product {
                Id = 17,
                Name = "Скрипач на крыше",
                Description = "Картина, изображающая традиционную еврейскую жизнь с музыкой и танцами.",
                Price = 2000.00m,
                StockQuantity = 4,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image17
            },
            new Product {
                Id = 18,
                Name = "Шум моря",
                Description = "Картина Каспара Давида Фридриха, изображающая морской пейзаж.",
                Price = 2300.00m,
                StockQuantity = 3,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image18
            },
            new Product {
                Id = 19,
                Name = "Площадь на восходе",
                Description = "Картина Эдварда Хоппера, изображающая пустую площадь в раннее утро.",
                Price = 1900.00m,
                StockQuantity = 5,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image19
            },
            new Product {
                Id = 20,
                Name = "Дети на пляже",
                Description = "Картина Пьера-Огюста Ренуара, изображающая детей, играющих на пляже.",
                Price = 2200.00m,
                StockQuantity = 4,
                CreatedAt = new DateTime (2025, 2, 16),
                Image = image20
            }
        );

        
        builder.Entity<ProductInfo> ().HasData (
            new ProductInfo {
                Id = 1,
                ProductId = 1,
                Technique = "Масло на холсте",
                Material = "Холст, масло",
                Plot = "Вид из окна в ночное время",
                Style = "Постимпрессионизм",
                Height = 73,
                Wight = 92,
                Year = 1889
            },
            new ProductInfo {
                Id = 2,
                ProductId = 2,
                Technique = "Масло на холсте",
                Material = "Холст, масло",
                Plot = "Автопортрет художника",
                Style = "Постимпрессионизм",
                Height = 64,
                Wight = 53,
                Year = 1889
            },
            new ProductInfo {
                Id = 3,
                ProductId = 3,
                Technique = "Масло на дереве",
                Material = "Дерево, масло",
                Plot = "Женщина с загадочной улыбкой",
                Style = "Ренессанс",
                Height = 77,
                Wight = 53,
                Year = 1503
            },
             new ProductInfo {
                 Id = 4,
                 ProductId = 4,
                 Technique = "Масло на холсте",
                 Material = "Холст, масло",
                 Plot = "Тайная вечеря",
                 Style = "Ренессанс",
                 Height = 460,
                 Wight = 880,
                 Year = 1495
             },
        new ProductInfo {
            Id = 5,
            ProductId = 5,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Женщина с жемчужной сережкой",
            Style = "Барокко",
            Height = 44,
            Wight = 39,
            Year = 1665
        },
        new ProductInfo {
            Id = 6,
            ProductId = 6,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Крик",
            Style = "Экспрессионизм",
            Height = 91,
            Wight = 73,
            Year = 1893
        },
        new ProductInfo {
            Id = 7,
            ProductId = 7,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Зеленая поляна",
            Style = "Импрессионизм",
            Height = 100,
            Wight = 81,
            Year = 1887
        },
        new ProductInfo {
            Id = 8,
            ProductId = 8,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Импрессия. Восходящее солнце",
            Style = "Импрессионизм",
            Height = 50,
            Wight = 65,
            Year = 1872
        },
        new ProductInfo {
            Id = 9,
            ProductId = 9,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Рождение Венеры",
            Style = "Ренессанс",
            Height = 172,
            Wight = 278,
            Year = 1486
        },
        new ProductInfo {
            Id = 10,
            ProductId = 10,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Ночной дозор",
            Style = "Барокко",
            Height = 363,
            Wight = 437,
            Year = 1642
        },
        new ProductInfo {
            Id = 11,
            ProductId = 11,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Три музыканта",
            Style = "Кубизм",
            Height = 107,
            Wight = 117,
            Year = 1921
        },
        new ProductInfo {
            Id = 12,
            ProductId = 12,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Герника",
            Style = "Кубизм",
            Height = 349,
            Wight = 777,
            Year = 1937
        },
        new ProductInfo {
            Id = 13,
            ProductId = 13,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Космический взгляд",
            Style = "Абстракционизм",
            Height = 152,
            Wight = 244,
            Year = 1947
        },
        new ProductInfo {
            Id = 14,
            ProductId = 14,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Завтрак на траве",
            Style = "Реализм",
            Height = 39,
            Wight = 59,
            Year = 1863
        },
        new ProductInfo {
            Id = 15,
            ProductId = 15,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Утро в Монте Карло",
            Style = "Пуантилизм",
            Height = 57,
            Wight = 70,
            Year = 1886
        },
        new ProductInfo {
            Id = 16,
            ProductId = 16,
            Technique = "Гравюра",
            Material = "Холст, гравюра",
            Plot = "Большая волна в Канагаве",
            Style = "Укиё-э",
            Height = 25,
            Wight = 37,
            Year = 1831
        },
        new ProductInfo {
            Id = 17,
            ProductId = 17,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Скрипач на крыше",
            Style = "Русский авангард",
            Height = 72,
            Wight = 51,
            Year = 1917
        },
        new ProductInfo {
            Id = 18,
            ProductId = 18,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Шум моря",
            Style = "Романтизм",
            Height = 50,
            Wight = 80,
            Year = 1825
        },
        new ProductInfo {
            Id = 19,
            ProductId = 19,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Площадь на восходе",
            Style = "Модернизм",
            Height = 50,
            Wight = 70,
            Year = 1926
        },
        new ProductInfo {
            Id = 20,
            ProductId = 20,
            Technique = "Масло на холсте",
            Material = "Холст, масло",
            Plot = "Дети на пляже",
            Style = "Импрессионизм",
            Height = 46,
            Wight = 55,
            Year = 1883
        });

    }
}
