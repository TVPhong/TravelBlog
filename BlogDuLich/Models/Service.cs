using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlogDuLich.Models
{
    public class Service
    {
        private readonly string _dataFile = @"Data\data.xml";
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(HashSet<Travel>));
        public HashSet<Travel> Travels { get; set; }
        public Service()
        {
            if (!File.Exists(_dataFile))
            {
                Travels = new HashSet<Travel>()
                {
                    new Travel {Id=1, Name="Phố cổ Hội An", Adress="Quảng Nam, Việt Nam", Year=1999,
                    Descriptions="Hội An là một thành phố trực thuộc tỉnh Quảng Nam, Việt Nam. Phố cổ Hội An từng là một thương cảng quốc tế sầm uất, gồm những di sản kiến trúc đã có từ hàng trăm năm trước, được UNESCO công nhận là di sản văn hóa thế giới từ năm 1999.",
                    Emotions="Thích những lúc đi bộ dạo quanh khu phố đèn lồng đầy đủ sắc màu, thích ngắm nhìn những tòa nhà cổ kính trăm tuổi, ánh đèn lung linh trên sông Hoài, thích thưởng thức cao lầu và hến trộn ở cầu Cẩm Nam và thích luôn cả những âm vực nghe có vẻ nặng nề nhưng chứa đựng sự chân chất và mộc mạc đến lạ thường."
                    },
                    new Travel{Id=2, Name="Vịnh Hạ Long", Adress="Quảng Ninh,Việt Nam", Year=1994,
                    Descriptions="Vịnh Hạ Long là một vịnh nhỏ thuộc phần bờ tây vịnh Bắc Bộ tại khu vực biển Đông Bắc Việt Nam, bao gồm vùng biển đảo của thành phố Hạ Long thuộc tỉnh Quảng Ninh.",
                    Emotions="Một lần đến Vịnh hạ Long tôi đã tham quan và khám phá các Hòn Trống Mái, Hòn Con Cóc, Hang Sửng Sốt… trải nghiệm chèo thuyền kayak quanh các hòn đảo chiêm ngưỡng vẻ đẹp của rừng và biển. "
                    },
                    new Travel{Id=3, Name="Hồ Gươm", Adress="Hà Nội, Việt Nam",
                    Descriptions="Hồ Hoàn Kiếm còn được gọi là Hồ Gươm là một hồ nước ngọt tự nhiên nằm ở trung tâm thành phố Hà Nội. Hồ có diện tích khoảng 12 ha. Trước kia, hồ còn có các tên gọi là hồ Lục Thủy, hồ Thủy Quân, hồ Tả Vọng và Hữu Vọng",
                    Emotions="Hồ Gươm như một lẵng hoa xinh đẹp nằm giữa lòng thành phố. Sự tích cái tên Hồ Gươm hay còn gọi là hồ Hoàn Kiếm đã gắn liền với gần ngàn năm lịch sử của đất Thăng Long."
                    }
                };

            }
            else
            {
                using var stream = File.OpenRead(_dataFile);
                Travels = _serializer.Deserialize(stream) as HashSet<Travel>;
            }
        }

        public Travel[] Get() => Travels.ToArray();
        public Travel Get(int id) => Travels.FirstOrDefault(trv => trv.Id == id);
        public bool Add(Travel travel) => Travels.Add(travel);
        public Travel Create()
        {
            var m = Travels.Max(trv => trv.Id);
            var trv = new Travel()
            {

                Id = m++
            };

            return trv;
        }

        public bool Update(Travel travel)
        {
            var trv = Get(travel.Id);
            return (trv != null && Travels.Remove(trv) && Travels.Add(travel));
        }

        public bool Delete(int id)
        {
            var trv = Get(id);
            return (trv != null && Travels.Remove(trv));
        }

        public void SaveChanges()
        {
            using var stream = File.Create(_dataFile);
            _serializer.Serialize(stream, Travels);
        }

        public string GetDataPath(string file) => $"Data\\{file}";

        public void Upload(Travel travel, IFormFile file)
        {
            if (file != null)
            {
                var path = GetDataPath(file.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);
                travel.DataFile = file.FileName;
            }
        }

        public (Stream, string) Download(Travel trv)
        {
            var memory = new MemoryStream();
            using var stream = new FileStream(GetDataPath(trv.DataFile), FileMode.Open);
            stream.CopyTo(memory);
            memory.Position = 0;
            var type = Path.GetExtension(trv.DataFile) switch
            {
                "pdf" => "application/pdf",
                "docx" => "application/vnd.ms-word",
                "doc" => "application/vnd.ms-word",
                "txt" => "text/plain",
                _ => "application/pdf"
            };
            return (memory, type);
        }

        public Travel[] Get(string search)
        {
            string s = search.ToLower();
            return Travels.Where(trv =>
                trv.Name.Contains(s) ||
                trv.Adress.Contains(s) ||
                trv.Descriptions.Contains(s) ||
                trv.Emotions.Contains(s) ||
                trv.Year.ToString() == s
            ).ToArray();
        }

        public (Travel[] travels, int pages, int page) Paging(int page)
        {
            int size = 5;
            int pages = (int)Math.Ceiling((double)Travels.Count / size);
            var travels = Travels.Skip((page - 1) * size).Take(size).ToArray();
            return (travels, pages, page);
        }


    }
}
