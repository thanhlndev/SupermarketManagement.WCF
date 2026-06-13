using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SupermarketManagement.WCF
{
    [ServiceContract]
    public interface ISieuThiService
    {
        [OperationContract]
        List<SieuthiDTO> GetAll();

        [OperationContract]
        SieuthiDTO GetById(string maST);

        [OperationContract]
        bool Insert(SieuthiDTO sieuthi);

        [OperationContract]
        bool Update(SieuthiDTO sieuthi);

        [OperationContract]
        List<SieuthiDTO> Search(string keyword);

        [OperationContract]
        bool Delete(string maST);
    }
    [DataContract]
    public class SieuthiDTO
    {
        [DataMember]
        public string MaST { get; set; }

        [DataMember]
        public string TenST { get; set; }

        [DataMember]
        public string DiaChi { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Sdt { get; set; }
    }
}
