namespace VPay.Payment.Api.Dtos
{
    public class ReasonCodeRequestDto
    {
        public ReasonCodeRequestDto()
        {
            User = "";
            PassWord = "";
            Txid = "";
            Source = ' ';
        }

        public string User { get; set; }
        public string PassWord { get; set; }
        public string Txid { get; set; }
        public char Source { get; set; } // Source of request P-webPage, S-webSvc, ' '-Other

    }
}
