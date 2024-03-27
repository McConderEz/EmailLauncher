using Email.BL;
using SMTPClient;
using System.Net.Mail;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Aspose.Email.Clients.Pop3;
using System.Runtime.CompilerServices;


string login = "";
string password = "";

#region ввод данных
//var data = Authorize();
//SMTPClient.SMTPClient client = new SMTPClient.SMTPClient(data.Item1, data.Item2, data.Item3, data.Item4);
#endregion

SMTPClient.SMTPClient client = new SMTPClient.SMTPClient("smtp.gmail.com", 587, login, password);
IMAPClient.IMAPClient imapClient = new IMAPClient.IMAPClient("imap.gmail.com",993,login, password);
POP3Client.POP3Client pop3Client = new POP3Client.POP3Client("pop.gmail.com", 995, login, password);


while (true)
{
    Console.WriteLine("1)Написать письмо");
    Console.WriteLine("2)Прочитать входящие письма");
    Console.WriteLine("3)Выход");
    Console.Write("Ваш ввод: ");
    int key = int.Parse(Console.ReadLine());

    if(key == 4)
    {
        Console.WriteLine("Завершение работы...");
        break;
    }

    switch (key)
    {
        case 1:
            Console.WriteLine("Введите путь к файлу(если не хотите - нажмите enter)");
            string path = Console.ReadLine();
            Console.WriteLine("Введите отображаемое имя(если не хотите - нажмите enter):");
            string displayName = Console.ReadLine();
            await client.SendEmail(CreateMessage(login), path, displayName);
            Console.WriteLine("Сообщение отправлено");
            Console.ReadKey();
            break;
        case 2:
            Console.Clear();
            Console.WriteLine("Выберите протокол:\n1)Imap\n2)Pop3");
            int protocolKey = int.Parse(Console.ReadLine());
            switch (protocolKey)
            {
                case 1:
                    Console.Write("Введите количество писем, которые хотите прочитать: ");
                    int count = int.Parse(Console.ReadLine());
                    imapClient.ReadMessages(count);
                    Console.Write("Введите id, чтобы прочитать содержимое письма(введите -1, если не желаете):");
                    int id = int.Parse(Console.ReadLine());
                    if (id == -1)
                        break;
                    else
                    {
                        Console.Clear();
                        imapClient.ReadMessageById(id);
                    }
                    break;
                case 2:
                    Console.Write("Введите количество писем, которые хотите прочитать: ");
                    int count2 = int.Parse(Console.ReadLine());
                    pop3Client.ReadMessages(count2);
                    Console.Write("Введите id, чтобы прочитать содержимое письма(введите -1, если не желаете):");
                    int id2 = int.Parse(Console.ReadLine());
                    if (id2 == -1)
                        break;
                    else
                    {
                        Console.Clear();
                        pop3Client.ReadMessageById(id2);
                    }
                    break;
            }
            Console.ReadKey();
            break;
        default:
            Console.WriteLine("Выбрана несуществующая операция");
            Console.ReadKey();
            break;
    }

    Console.Clear();
}

Console.ReadKey();


static IMessage CreateMessage(string login)
{
    Console.WriteLine("Введите почту получателя: ");
    string to = Console.ReadLine();

    Console.WriteLine("Введите тему письма:");
    string subject = Console.ReadLine();

    Console.WriteLine("Введите содержимое письма:");
    string text = Console.ReadLine();

    return new Message { From = login, To = to, Subject = subject, Text = text };
}



static (string, int, string, string) Authorize()
{
    string[] connectionStirng;
    while (true)
    {
        Console.WriteLine("Введите host, port, login, password: ");
        connectionStirng = Console.ReadLine().Split(" ");

        if (connectionStirng.Length == 4)
            break;
    }

    return (connectionStirng[0], int.Parse(connectionStirng[1]), connectionStirng[2], connectionStirng[3]);
}