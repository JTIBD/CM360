using FMAplication.Domain.Bases;

namespace FMAplication.Domain.AVCommunications
{
    public class CommunicationSetup : BaseSetup
    {
        public string Code { get; set; }
        public int AvCommunicationId { get; set; }
        public AvCommunication AvCommunication { get; set; }
    }
}
