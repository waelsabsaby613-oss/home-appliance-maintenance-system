using System;

namespace HomeApplianceMaintenance
{
    // 1. Abstraction: الواجهة المحددة للعمليات الأساسية
    public interface IMaintainable
    {
        void PerformMaintenance();
        string GetMaintenanceStatus();
    }

    // 1. Abstraction & Encapsulation: الفئة المجردة الرئيسية
    public abstract class Appliance : IMaintainable
    {
        // Encapsulation: حقول خاصة
        private readonly string _serialNumber; // قراءة فقط بعد الإنشاء
        private string _brand;
        private double _baseRepairCost;
        private bool _isRepaired;

        // Properties للتحكم بالوصول
        public string SerialNumber => _serialNumber; // Read-Only Property
        
        public string Brand
        {
            get => _brand;
            set => _brand = string.IsNullOrWhiteSpace(value) ? "Unknown Brand" : value;
        }

        public double BaseRepairCost
        {
            get => _baseRepairCost;
            protected set => _baseRepairCost = value < 0 ? 0 : value;
        }

        public bool IsRepaired
        {
            get => _isRepaired;
            protected set => _isRepaired = value;
        }

        // Constructor
        protected Appliance(string serialNumber, string brand, double baseRepairCost)
        {
            _serialNumber = serialNumber;
            Brand = brand;
            BaseRepairCost = baseRepairCost;
            _isRepaired = false;
        }

        // Abstract Method: دالة مجردة لحساب تكلفة الإصلاح الإجمالية
        public abstract double CalculateTotalRepairCost();

        // Interface Implementation
        public virtual void PerformMaintenance()
        {
            _isRepaired = true;
        }

        public virtual string GetMaintenanceStatus()
        {
            return _isRepaired ? "تم الإصلاح (Completed)" : "قيد الصيانة (Under using System;
using System.Collections.Generic;

namespace HomeApplianceMaintenance
{
    // 2. Polymorphism & Management: إدارة الأجهزة ومعالجتها عبر List
    public class MaintenanceManager
    {
        private readonly List<Appliance> _appliances = new List<Appliance>();

        public void AddAppliance(Appliance appliance)
        {
            if (appliance != null)
            {
                _appliances.Add(appliance);
            }
        }

        // استخدام Polymorphism لحساب التكاليف الإجمالية لجميع الأجهزة
        public double CalculateTotalCenterRevenue()
        {
            double total = 0;
            foreach (var app in _appliances)
            {
                total += app.CalculateTotalRepairCost(); // Polymorphic Call
            }
            return total;
        }

        // عرض حالات الأجهزة بالكامل
        public void DisplayAllAppliancesReport()
        {
            Console.WriteLine("\n==== تقرير مركز الصيانة الشامل ====");
            foreach (var app in _appliances)
            {
                Console.WriteLine($"[النوع: {app.GetType().Name}] | الرقم التسلسلي: {app.SerialNumber} | الماركة: {app.Brand} | الحالة: {app.GetMaintenanceStatus()} | التكلفة: {app.CalculateTotalRepairCost()}$");
            }
        }

        public List<Appliance> GetAppliances() => _appliances;
    }
}
using System.Text.RegularExpressions;

namespace HomeApplianceMaintenance
{
    // 4. Delegates & Events: التفويض والأحداث
    public delegate void StatusChangedHandler(string serialNumber, string newStatus);

    public class MaintenanceNotifier
    {
        // Event إطلاق حدث عند تغير حالة الصيانة
        public event StatusChangedHandler OnStatusChanged;

        public void NotifyStatusChange(string serialNumber, string newStatus)
        {
            // إطلاق الحدث عند تحقق الشرط
            OnStatusChanged?.Invoke(serialNumber, newStatus);
        }
    }

    // 5. Static Classes & Members: الفئات والأعضاء الثابتة
    public static class ValidationHelper
    {
        // دالة استاتيكية للتحقق من صحة الرقم التسلسلي (مثلاً يجب أن يبدأ بـ SN وحروف/أرقام)
        public static bool IsValidSerialNumber(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber)) return false;
            return Regex.IsMatch(serialNumber, @"^SN-[0-9]{4}$");
        }
    }

    public static class MaintenanceStatistics
    {
        // خاصية استاتيكية لحساب عدد الأجهزة المسجلة
        public static int TotalRegisteredDevices { get; set; } = 0;
    }
}
using System;

namespace HomeApplianceMaintenance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== نظام إدارة مركز صيانة الأجهزة المنزلية ===");

            MaintenanceManager manager = new MaintenanceManager();
            MaintenanceNotifier notifier = new MaintenanceNotifier();

            // الاشتراك في الحدث Event Subscription
            notifier.OnStatusChanged += (sn, status) => 
            {
                Console.WriteLine($"[إشعار الحدث 🔔]: الجهاز {sn} تغيرت حالته إلى: {status}");
            };

            // إنشاء أجهزة واختبار الـ Static Validation
            string sn1 = "SN-1001";
            if (ValidationHelper.IsValidSerialNumber(sn1))
            {
                var fridge = new Refrigerator(sn1, "LG", 200, true);
                manager.AddAppliance(fridge);
                MaintenanceStatistics.TotalRegisteredDevices++;
            }

            string sn2 = "SN-1002";
            if (ValidationHelper.IsValidSerialNumber(sn2))
            {
                var washer = new WashingMachine(sn2, "Samsung", 150, true);
                manager.AddAppliance(washer);
                MaintenanceStatistics.TotalRegisteredDevices++;
            }

            string sn3 = "SN-1003";
            if (ValidationHelper.IsValidSerialNumber(sn3))
            {
                var ac = new AirConditioner(sn3, "Gree", 180, 24000);
                manager.AddAppliance(ac);
                MaintenanceStatistics.TotalRegisteredDevices++;
            }

            // تنفيذ إجراءات صيانة وإطلاق الحدث
            var list = manager.GetAppliances();
            if(list.Count > 0)
            {
                list[0].PerformMaintenance();
                notifier.NotifyStatusChange(list[0].SerialNumber, list[0].GetMaintenanceStatus());
            }

            // طباعة التقارير وحساب التكاليف الإجمالية باستخدام Polymorphism
            manager.DisplayAllAppliancesReport();

            Console.WriteLine($"\nإجمالي عدد الأجهزة المسجلة (Static Property): {MaintenanceStatistics.TotalRegisteredDevices}");
            Console.WriteLine($"إجمالي إيرادات مركز الصيانة: {manager.CalculateTotalCenterRevenue()}$");

            Console.WriteLine("\nاضغط أي مفتاح للخروج...");
            Console.ReadKey();
        }
    }
}
