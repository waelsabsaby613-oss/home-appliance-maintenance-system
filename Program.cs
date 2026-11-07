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