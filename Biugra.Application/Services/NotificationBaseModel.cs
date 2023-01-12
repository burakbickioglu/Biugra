//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Biugra.Service.Services
//{
//    public class NotificationSenderModel
//    {
//        public string MailSubject { get; set; }
//        public string MailBody { get; set; }
//        public string SmsBody { get; set; }
//    }
//    public class NotificationBaseModel
//    {
//        public NotificationBaseModel(string templateType, string layoutTemplateType)
//        {
//            TemplateType = templateType;
//            LayoutTemplateType = layoutTemplateType;
//        }
//        public string TemplateType { get; set; }
//        public string LayoutTemplateType { get; set; }
//        public Dictionary<string, string> Props { get; set; }
//        internal void AssignProps()
//        {
//            Props = new Dictionary<string, string>();

//            var propNames = new NotificationBaseModel("", "").GetType().GetProperties().Select(x => x.Name);
//            Type genericType = this.GetType();
//            object classObject = this;
//            var props = genericType.GetProperties().Where(x => !propNames.Contains(x.Name) && !string.IsNullOrEmpty((string)x.GetValue(classObject)));

//            foreach (var prop in props)
//            {
//                Props.Add(prop.Name, (string)prop.GetValue(classObject));
//            }
//        }
//        public NotificationSenderModel ToNotificationSenderModel(GeneralNotificationContent notificationTemps) // mail listesi
//        {
//            var notificationCheck = false;
//            var mailLayoutTemplateGeneralModel = notificationTemps.MailTemplate?.FirstOrDefault(x => x.Key == LayoutTemplateType);
//            var mailTemplateGeneralModel = notificationTemps.MailTemplate?.FirstOrDefault(x => x.Key == TemplateType);
//            var smsTemplateGeneralModel = notificationTemps.SmsTemplate?.FirstOrDefault(x => x.Key == TemplateType);

//            string mailLayout = "";
//            string mailBody = "";
//            string mailSubject = "";
//            string smsBody = "";

//            if (mailLayoutTemplateGeneralModel?.Value is not null)
//            {
//                var mailLayoutbase = JsonConvert.DeserializeObject<GeneralContentDetailDTO>(mailLayoutTemplateGeneralModel.Value.Value.ToString());
//                mailLayout = mailLayoutbase?.Content ?? string.Empty;
//            }

//            if (mailTemplateGeneralModel?.Value is not null)
//            {
//                var mailtempLate = JsonConvert.DeserializeObject<GeneralContentDetailDTO>(mailTemplateGeneralModel.Value.Value.ToString());
//                mailBody = mailtempLate?.Content ?? string.Empty;
//                mailSubject = mailtempLate?.Title ?? string.Empty;
//                notificationCheck = mailtempLate != null;
//            }

//            if (smsTemplateGeneralModel?.Value is not null)
//            {
//                var smsBodybase = JsonConvert.DeserializeObject<GeneralContentDetailDTO>(smsTemplateGeneralModel.Value.Value.ToString());
//                smsBody = smsBodybase?.Content ?? string.Empty;
//                notificationCheck = !string.IsNullOrEmpty(smsBody);
//            }

//            if (!notificationCheck)
//                return null;

//            foreach (var prop in Props)
//            {
//                var value = prop.Value;
//                var name = prop.Key;
//                if (!string.IsNullOrEmpty(mailBody) && mailBody.Contains($"??{prop.Key}??"))
//                {
//                    mailBody = mailBody.Replace($"??{name}??", value);
//                }
//                if (!string.IsNullOrEmpty(mailSubject) && mailSubject.Contains($"??{name}??"))
//                {
//                    mailSubject = mailSubject.Replace($"??{name}??", value);
//                }

//            }

//            if (!string.IsNullOrEmpty(mailBody) && !string.IsNullOrEmpty(mailLayout))
//                mailBody = mailLayout.Replace("@CONTENT", mailBody);


//            return new NotificationSenderModel { MailBody = mailBody, MailSubject = mailSubject, SmsBody = smsBody };
//        }

//    }
//    public class ForgetPassword : NotificationBaseModel
//    {
//        public ForgetPassword(string templateType, string layoutTemplateType, string email, string url) : base(templateType, layoutTemplateType)
//        {
//            this.email = email;
//            this.url = url;
//            AssignProps();
//        }

//        public string email { get; set; }
//        public string url { get; set; }

//    }
//    public class ConfirmMail : NotificationBaseModel
//    {
//        public ConfirmMail(string templateType, string layoutTemplateType, string email) : base(templateType, layoutTemplateType)
//        {
//            Email = email;
//            AssignProps();

//        }

//        public string Email { get; set; }
//    }
//}
