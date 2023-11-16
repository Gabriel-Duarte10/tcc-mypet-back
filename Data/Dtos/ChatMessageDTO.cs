using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Dtos
{
    public class ChatMessageDTO
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public int SessionId { get; set; } // Adicionado SessionId
        public int? PetId { get; set; }
        public int? ProductId { get; set; }
        public string? Text { get; set; }
        public string? Image64 { get; set; }
        public int SenderUser { get; set; }
        public int ReceiverUser { get; set; }
    }
}