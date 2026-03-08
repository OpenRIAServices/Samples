using System.Runtime.Serialization;

namespace SimpleApplication.Web.Model
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/ServerSideAsyncDomainService")]
    public class ComplexType2
    {
        [DataMember]
        public int A { get; set; }

        [DataMember]
        public string B { get; set; }
    }
}