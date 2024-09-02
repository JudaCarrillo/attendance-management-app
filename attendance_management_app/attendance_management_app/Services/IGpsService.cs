namespace attendance_management_app.Services
{
    public interface IGpsService
    {
        void OpenSettings();   // Método para abrir la configuración de GPS
        bool IsGpsEnabled();   // Método para verificar si el GPS está habilitado
        (double Latitude, double Longitude) GetLocation(); // Método para obtener latitud y longitud
    }
}
