<h1 align="center">🪶 CertBlocker Lite</h1>

<p align="center">
  Минимальная версия <b>CertBlocker</b> — блокировка недоверенных сертификатов
  <b>без прав администратора</b> и без доступа к Настройкам.
  Для сильно ограниченных учётных записей.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/platform-Windows%207–11-0078D6?logo=windows&logoColor=white" alt="Platform">
  <img src="https://img.shields.io/badge/C%23-.NET%20Framework%204.x-512BD4?logo=csharp&logoColor=white" alt="C#">
  <img src="https://img.shields.io/badge/no%20admin-✅-2ECC71" alt="No admin">
  <img src="https://img.shields.io/badge/license-MIT-2ECC71" alt="License: MIT">
</p>

<p align="center">
  🔗 Полные версии: <a href="https://github.com/1565gfd/CertBlocker">CertBlocker</a> ·
  <a href="https://github.com/1565gfd/CertBlocker-NoAdmin">CertBlocker-NoAdmin</a>
</p>

---

## 📖 Что это

Одно маленькое окно: **заблокировать сертификат из файла**, список заблокированных,
**разблокировать**. Пишет в хранилище `CurrentUser\Disallowed` — работает правами
обычного пользователя, **UAC и Настройки не нужны**.

## 🚀 Запуск

Скачайте `CertBlockerLite.exe` из **[Releases](../../releases)** и запустите двойным
кликом. Права администратора не требуются.

> ⚠️ Работает, если на компьютере **разрешён запуск .exe**. Если запуск программ
> заблокирован политикой (AppLocker/SRP) — обойти это приложением нельзя.

При предупреждении **SmartScreen**: «Подробнее» → «Выполнить в любом случае».

**SHA-256** (v1.0.0):
```
6A90B11BB3FB480FCB73722889FE1EACAB035670C3BE607D42AA13F2BB762739
```

## 🛠️ Сборка

```powershell
powershell -ExecutionPolicy Bypass -File .\build.ps1
```

## 🔐 Безопасность

Не выходит в сеть, не запускает внешних команд. Единственное действие — запись в
`CurrentUser\Disallowed`, отменяемая кнопкой «Разблокировать». Есть защита от
подмены DLL, при загрузке файла берётся только публичная часть сертификата.

## 📄 Лицензия

[MIT](LICENSE)
