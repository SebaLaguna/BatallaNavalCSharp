using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "foto".
    /// </summary>
    public class HelpHandler : BaseHandler
    {
        private TelegramBotClient bot;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PhotoHandler"/>. Esta clase procesa el mensaje "foto".
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        /// <param name="bot">El bot para enviar la foto.</param>
        public HelpHandler(TelegramBotClient bot, BaseHandler next)
            : base(new string[] { "help" }, next)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Procesa el mensaje "foto" y retorna true; retorna false en caso contrario.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override void InternalHandle(Message message, out string response,out ReplyKeyboardMarkup reply)
        {
            reply = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Register")

            }
        });
            response = "Aqui llegó la Ayuda";
            SendProfileImage(message);
        }

        /// <summary>
        /// Envía una imagen como respuesta al mensaje recibido. Como ejemplo enviamos siempre la misma foto.
        /// </summary>
        private async Task SendProfileImage(Message message)
        {       
            // Can be null during testing
            if (bot != null)
            {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

            const string filePath = @"../../docs/profile.jpeg";
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

            await bot.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: InputFile.FromStream(fileStream, fileName),
                caption: ""
            );
            }
        }
    }
