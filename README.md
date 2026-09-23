# CertBlocker-Lite

Минимальная версия [CertBlocker](https://github.com/1565gfd/CertBlocker) без прав
администратора. Одно окно: блокировка сертификата из файла, список заблокированных,
разблокировка. Запись в `CurrentUser\Disallowed`; UAC и доступ к Настройкам не нужны.

![platform](https://img.shields.io/badge/platform-Windows%207–11-0078D6?logo=windows&logoColor=white)
![framework](https://img.shields.io/badge/.NET%20Framework-4.x-512BD4)
![admin](https://img.shields.io/badge/admin-not%20required-2ECC71)
![license](https://img.shields.io/badge/license-MIT-2ECC71)

## Возможности

- Блокировка сертификата из файла (`.cer`, `.crt`, `.der`, `.pem`).
- Список заблокированных сертификатов и разблокировка.

## Требования

- Windows 7–11.
- .NET Framework 4.x (входит в состав Windows).

## Сборка

```powershell
powershell -ExecutionPolicy Bypass -File .\build.ps1
```

## Проверка целостности

```powershell
Get-FileHash -Algorithm SHA256 .\CertBlockerLite.exe
```

SHA-256 релиза **v1.0.1**:

```
8838EF8D037DE8232F7A0526877323A24C640C207644FC191B814FB3A981A2C8
```

## Примечания

- Приложение не подписано — возможно предупреждение SmartScreen при первом запуске.
- Запуск возможен, если в системе разрешено выполнение `.exe`. При действующей
  политике AppLocker/SRP запуск сторонних программ может быть заблокирован.

## Версии

- [CertBlocker](https://github.com/1565gfd/CertBlocker) — для всей системы, требуются права администратора.
- [CertBlocker-NoAdmin](https://github.com/1565gfd/CertBlocker-NoAdmin) — для текущего пользователя, без прав администратора.
- [CertBlocker-Lite](https://github.com/1565gfd/CertBlocker-Lite) — минимальная версия без прав администратора.

## Лицензия

[MIT](LICENSE)
