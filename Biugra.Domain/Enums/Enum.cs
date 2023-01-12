namespace Biugra.Domain.Enums;

public enum MailTypes
{
    WelcomeCustomer = 0,
    ResetPassword = 1
}
public enum ContentType
{
    Agreement = 0,
    ErrorMessage = 1,
    Icon = 2,
    File = 3,
    Mail = 4,
    Faq = 5,
    Sms = 6,
    SystemConfig = 7
}

public enum Gender
{
    [Display(Name ="Erkek")]
    Erkek = 0, // erkek
    [Display(Name = "Kadın")]
    Kadın = 1, // kadın
    [Display(Name = "Belirtmek İstemiyorum")]
    Belirtilmemiş = 2
}
//public enum TestStatus
//{
//    Draft = 0,
//    Live = 1,
//    Done = 2
//}

//public enum FieldTypes
//{
//    Label = 0,
//    TextField = 1,
//    TextArea = 2,
//    Button = 3,
//    Combobox = 4,
//    RadioButton = 5,
//    NumericTextBox = 6,
//    Image = 7,
//    File = 8
//}

//public enum TestTypes
//{
//    Blank = 0,
//    FiveSecondTest = 1,
//    CardSorting = 2,
//    FirstClickTest = 3,
//    TreeTest = 4
//}
