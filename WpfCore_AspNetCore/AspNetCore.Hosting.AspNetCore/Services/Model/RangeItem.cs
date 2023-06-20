using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SimpleApplication.Web.Model
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/ServerSideAsyncDomainService")]
    public class RangeItem
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}