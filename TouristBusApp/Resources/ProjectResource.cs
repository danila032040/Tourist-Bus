using System.IO;
using System.Linq;
using TouristBusApp.Models;
using TouristBusApp.Services;
using TouristBusApp.Services.Repositories;

namespace TouristBusApp.Resources
{
    public class ProjectResource
    {
        /// <summary>
        ///     Статическая ссылка на ProjectResource
        /// </summary>
        /// <remarks>
        ///     Необходимо для реализации паттерна Singleton
        /// </remarks>
        private static ProjectResource _singleTon;

        /// <summary>
        ///     Класс авторизации
        /// </summary>
        public readonly Authentication Authentication;

        /// <summary>
        ///     Репозиторий автобусов
        /// </summary>
        public readonly IRepository<Bus> BussesRep;

        /// <summary>
        ///     Репозиторий дорог
        /// </summary>
        public readonly IRepository<Road> RoadsRep;

        /// <summary>
        ///     Репозиторий точек тура
        /// </summary>
        public readonly IRepository<TourPoint> TourPointsRep;

        /// <summary>
        ///     Репозиторий туров
        /// </summary>
        public readonly IRepository<Tour> ToursRep;

        /// <summary>
        ///     Репозиторий пользователей
        /// </summary>
        public readonly IRepository<User> UsersRep;


        public ProjectResource()
        {
            #region Создание файлов/папок, необходимых для корректной работы репозиториев

            Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\Data");
            if (!File.Exists(UsersFileName)) File.Create(UsersFileName);
            if (!File.Exists(ToursFileName)) File.Create(ToursFileName);
            if (!File.Exists(TourPointsFileName)) File.Create(TourPointsFileName);
            if (!File.Exists(RoadsFileName)) File.Create(RoadsFileName);

            #endregion

            #region Инициализация репозиториев

            UsersRep = new UserRepository(UsersFileName);
            BussesRep = new BusRepository(BussesResourceName);
            RoadsRep = new RoadRepository(RoadsFileName);
            TourPointsRep = new TourPointRepository(TourPointsFileName);
            ToursRep = new TourRepository(ToursFileName);
            Authentication = new Authentication(UsersRep);

            #endregion

            #region Инициализация данных внутри репозиториев

            if (UsersRep.Read().Count() == 0 || !UsersRep.Read().Any(u => u.Login == "admin" && u.Password == "admin"))
                UsersRep.Create(new User
                {
                    Login = "admin",
                    Password = "admin",
                    Role = UserRole.Admin
                });
            
            if (TourPointsRep.Read().Count() == 0 || !TourPointsRep.Read().Any(tp=>tp.Name == "Стартовая станция \"Успех\""))
                TourPointsRep.Create(new TourPoint
                {
                    Name="Стартовая станция \"Успех\""
                });
            
            #endregion
        }

        /// <summary>
        ///     Свойство возвращающая экземпляр ProjectResource, создавая его, если он до этого не был создан
        /// </summary>
        /// <remarks>
        ///     Необходимо для реализации паттерна Singleton
        /// </remarks>
        public static ProjectResource Instance
        {
            get { return _singleTon ??= new ProjectResource(); }
        }

        /// <summary>
        ///     Ссылка на json файл пользователей
        /// </summary>
        private string UsersFileName => $@"{Directory.GetCurrentDirectory()}\Data\Users.json";

        /// <summary>
        ///     Ссылка на json файл туров
        /// </summary>
        private string ToursFileName => $@"{Directory.GetCurrentDirectory()}\Data\Tours.json";

        /// <summary>
        ///     Ссылка на json файл точек тура
        /// </summary>
        private string TourPointsFileName => $@"{Directory.GetCurrentDirectory()}\Data\TourPoints.json";

        /// <summary>
        ///     Ссылка на json файл дорог
        /// </summary>
        private string RoadsFileName => $@"{Directory.GetCurrentDirectory()}\Data\TourRoads.json";

        /// <summary>
        ///     Название файла ресурсов, содержащего автобусы
        /// </summary>
        private string BussesResourceName => "Resources.Busses.json";
    }
}