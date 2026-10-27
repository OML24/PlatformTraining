# منصة التدريب والتطوير المهني — جامعة الزرقاء

مشروع .NET 10 وفق Clean Architecture مع PostgreSQL وEF Core. WebPortal ملفات ثابتة بـBootstrap 5 وjQuery/AJAX، بلا Razor. مصدر المتطلبات في [PLATFORM_SPECIFICATION.md](../PLATFORM_SPECIFICATION.md)، وحالة التنفيذ الدقيقة في [WORK_PROGRESS.md](WORK_PROGRESS.md).

## المشروعات

| المشروع | المسؤولية |
| --- | --- |
| `ZU.Training.Domain` | الكيانات وقواعد الحضور واستحقاق الشهادة |
| `ZU.Training.Application` | حالات الاستخدام والعقود |
| `ZU.Training.Infrastructure` | EF Core، PostgreSQL، التخزين والتكاملات |
| `ZU.Training.WebApi` | REST API والمصادقة والصلاحيات |
| `ZU.Training.WebPortal` | الواجهة الثابتة العربية/الإنجليزية |
| `ZU.Training.Domain.Tests` | اختبارات قواعد الأعمال والتخزين |

## التشغيل المحلي

يتطلب .NET 10 وPostgreSQL. احفظ `ConnectionStrings:TrainingDb` و`Security:EncryptionKey` و`Security:JwtSigningKey` خارج المستودع في User Secrets لمشروع WebApi. لا تنسخ أسرار التطوير إلى إعدادات الإنتاج.

```powershell
dotnet restore ZU.Training.sln
dotnet tool restore
dotnet tool run dotnet-ef database update --project src/ZU.Training.Infrastructure --startup-project src/ZU.Training.WebApi --context TrainingDbContext
dotnet test ZU.Training.sln
dotnet run --project src/ZU.Training.WebApi --urls http://127.0.0.1:5098
dotnet run --project src/ZU.Training.WebPortal --urls http://127.0.0.1:5099
```

البوابة المحلية على `http://127.0.0.1:5099/`. هذه **نسخة تطوير قيد التنفيذ**؛ راجع `WORK_PROGRESS.md` قبل أي محاولة نشر.
