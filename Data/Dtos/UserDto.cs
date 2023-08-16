namespace tcc_mypet_back.Data.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cellphone { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public List<UserImageDto> UserImages { get; set; }
    }

    public class UserImageDto
    {
        public int UserId { get; set; }
        public string ImageName { get; set; }
        public string Image64 { get; set; }
    }
}
