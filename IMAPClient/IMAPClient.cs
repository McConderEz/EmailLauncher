using MailKit.Net.Imap;
using MimeKit;
using MimeKit.Tnef;


namespace IMAPClient
{
    public class IMAPClient
    {
        private ImapClient _imapClient;
        public IMAPClient(string address = "imap.gmail.com", int port = 993, string login = null, string password = null)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException(nameof(address));

            if (string.IsNullOrWhiteSpace(login)) throw new ArgumentNullException(nameof(login));

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            _imapClient = new ImapClient();
            _imapClient.Connect(address, port, true);
            _imapClient.Authenticate(login, password);

        }


        public void ReadMessages(int c = 10)
        {
            var inbox = _imapClient.Inbox;
            inbox.Open(MailKit.FolderAccess.ReadOnly);

            int count = inbox.Count > c ? c : inbox.Count;

            for (int i = inbox.Count - count; i < inbox.Count; i++)
            {
                var message = inbox.GetMessage(i);
                Console.WriteLine($"ID: {i}\nSubject: {message.Subject}\nFrom: {message.From}\nTo: {message.To}");
                Console.WriteLine(new string('-',20));
            }

        }

        public void ReadMessageById(int id)
        {
            var inbox = _imapClient.Inbox;
            inbox.Open(MailKit.FolderAccess.ReadWrite);

            if (id >= 0 && id < inbox.Count)
            {
                var message = inbox.GetMessage(id);
                Console.WriteLine("Subject: " + message.Subject);
                Console.WriteLine("From: " + message.From);
                Console.WriteLine("To: " + message.To);
                Console.WriteLine("Body: " + message.TextBody);

                foreach (var attachment in message.Attachments)
                {
                    var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                    using (var stream = System.IO.File.Create(Environment.CurrentDirectory + "\\" + fileName))
                    {
                        if (attachment is MimePart)
                        {
                            var part = (MimePart)attachment;
                            part.Content.DecodeTo(stream);
                        }
                    }
                    Console.WriteLine("Прикрепленные: " + fileName);
                }

            }
        }
    }
}
