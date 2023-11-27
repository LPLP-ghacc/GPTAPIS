namespace GPTAPIS.MessageConstruct.Text;

public enum Model
{
    GPT4_1106_Preview,
    GPT4_Vision_Preview,
    GPT4,
    GPT4_32K,
    GPT3_5_Turbo,
    GPT3_5_Turbo_16K,
    Davinci,
    DavinciEdit,
    Curie,
    Babbage,
    Ada,
    Embedding_Ada_002,
    Whisper1,
    Moderation_Latest
}

public static class ModelConvert
{
    public static string GetModel(Model model)
    {
        switch (model)
        {
            case Model.GPT4_1106_Preview:
                // Модель предварительного просмотра GPT-4 с ограниченным доступом
                return "gpt-4-1106-preview";
            case Model.GPT4_Vision_Preview:
                // Модель предварительного просмотра GPT-4 с зрительными возможностями
                return "gpt-4-vision-preview";
            case Model.GPT4:
                // Модель GPT-4
                return "gpt-4";
            case Model.GPT4_32K:
                // Модель GPT-4 с 32к словами
                return "gpt-4-32k";
            case Model.GPT3_5_Turbo:
                // Более быстрая и экономичная альтернатива модели GPT-3.5
                return "gpt-3.5-turbo";
            case Model.GPT3_5_Turbo_16K:
                // Модель GPT-3.5 Turbo с 16k словами
                return "gpt-3.5-turbo-16k";
            case Model.Davinci:
                // Самая мощная модель Davinci
                return "text-davinci-003";
            case Model.DavinciEdit:
                // Модель Davinci, ориентированная на редактирование текста
                return "text-davinci-edit-001";
            case Model.Curie:
                // Модель Curie для задач средней сложности
                return "text-curie-001";
            case Model.Babbage:
                // Модель Babbage для простых задач
                return "text-babbage-001";
            case Model.Ada:
                // Модель Ada для очень простых и быстрых задач
                return "text-ada-001";
            case Model.Embedding_Ada_002:
                // Модель Ada для встраивания текста (embedding)
                return "text-embedding-ada-002";
            case Model.Whisper1:
                // Модель Whisper для распознавания и перевода речи
                return "whisper-1";
            case Model.Moderation_Latest:
                // Самая последняя модель для модерации контента
                return "text-moderation-latest";
            default:
                // Возвращает значение по умолчанию, если не соответствует никакому известному перечислению
                return "gpt-3.5-turbo";
        }
    }
}
