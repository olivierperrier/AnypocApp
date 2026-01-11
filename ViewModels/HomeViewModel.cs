using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AnypocApp.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<MedicalTestResult> _testResults = new();

    public HomeViewModel()
    {
        // Sample medical test results
        TestResults.Add(new MedicalTestResult { Type = "Blood Glucose", Value = "95", Unit = "mg/dL", PatientId = "P001" });
        TestResults.Add(new MedicalTestResult { Type = "Cholesterol", Value = "180", Unit = "mg/dL", PatientId = "P001" });
        TestResults.Add(new MedicalTestResult { Type = "Hemoglobin A1C", Value = "5.6", Unit = "%", PatientId = "P002" });
        TestResults.Add(new MedicalTestResult { Type = "Blood Pressure", Value = "120/80", Unit = "mmHg", PatientId = "P002" });
        TestResults.Add(new MedicalTestResult { Type = "INR", Value = "1.1", Unit = "", PatientId = "P003" });
        TestResults.Add(new MedicalTestResult { Type = "Creatinine", Value = "1.0", Unit = "mg/dL", PatientId = "P003" });
        TestResults.Add(new MedicalTestResult { Type = "Troponin", Value = "0.02", Unit = "ng/mL", PatientId = "P001" });
        TestResults.Add(new MedicalTestResult { Type = "CRP", Value = "2.5", Unit = "mg/L", PatientId = "P004" });
    }
}

public class MedicalTestResult
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
}
