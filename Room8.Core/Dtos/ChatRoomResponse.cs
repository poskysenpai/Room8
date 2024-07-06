namespace Room8.Core.Dtos
{
    public class ChatRoomResponse
    {
        public long Id { get; set; }
        public string User1Id { get; set; }
        public string User1Name { get; set; }
        public string User1ProfileUrl { get; set; } = "";
        public string User2Id { get; set; }
        public string User2Name { get; set; }
        public string User2ProfileUrl { get; set; } = "";
    }
}
