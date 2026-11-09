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
