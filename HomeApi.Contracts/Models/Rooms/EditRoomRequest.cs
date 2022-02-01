namespace HomeApi.Contracts.Models.Devices
{
    /// <summary>
    /// Запрос для обновления свойств подключенного устройства
    /// </summary>
    public class EditRoomRequest
    {
        public string NewName { get; set; }
    }
}