// CertBlocker Lite - минимальная версия без прав администратора.
// Блокирует сертификат из файла через CurrentUser\Disallowed (без UAC и Настроек).
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace CertBlockerLite
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDefaultDllDirectories(uint f);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string p);

        [STAThread]
        static void Main()
        {
            try { SetDefaultDllDirectories(0x800); } catch { }
            try { SetDllDirectory(string.Empty); } catch { }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LiteForm());
        }
    }

    class LiteForm : Form
    {
        readonly ListView list = new ListView();
        readonly Label status = new Label();

        static X509Store Store()
        {
            return new X509Store(StoreName.Disallowed, StoreLocation.CurrentUser);
        }
        static string Name(X509Certificate2 c)
        {
            if (!string.IsNullOrEmpty(c.FriendlyName)) return c.FriendlyName;
            var n = c.GetNameInfo(X509NameType.SimpleName, false);
            return string.IsNullOrEmpty(n) ? c.Subject : n;
        }

        public LiteForm()
        {
            Text = "CertBlocker Lite";
            ClientSize = new Size(560, 420);
            MinimumSize = new Size(460, 320);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9F);

            var lbl = new Label
            {
                Text = "Блокировка сертификатов без прав администратора (для текущего пользователя).",
                Location = new Point(12, 10),
                Size = new Size(536, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(lbl);

            var btnAdd = new Button { Text = "Заблокировать из файла…", Location = new Point(12, 36), Size = new Size(200, 30) };
            btnAdd.Click += (s, e) => BlockFromFile();
            Controls.Add(btnAdd);

            var btnRefresh = new Button { Text = "Обновить", Location = new Point(220, 36), Size = new Size(100, 30) };
            btnRefresh.Click += (s, e) => Reload();
            Controls.Add(btnRefresh);

            list.Location = new Point(12, 74);
            list.Size = new Size(536, 300);
            list.View = View.Details; list.FullRowSelect = true; list.GridLines = true; list.MultiSelect = false;
            list.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            list.Columns.Add("Заблокированный сертификат", 340);
            list.Columns.Add("Отпечаток", 190);
            Controls.Add(list);

            var btnRemove = new Button
            {
                Text = "Разблокировать выбранный",
                Location = new Point(12, 382),
                Size = new Size(220, 28),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            btnRemove.Click += (s, e) => Unblock();
            Controls.Add(btnRemove);

            status.Location = new Point(240, 386);
            status.Size = new Size(308, 22);
            status.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            status.ForeColor = Color.FromArgb(0, 100, 0);
            Controls.Add(status);

            Reload();
        }

        void SetStatus(string t, bool err)
        {
            status.Text = t;
            status.ForeColor = err ? Color.FromArgb(160, 0, 0) : Color.FromArgb(0, 100, 0);
        }

        void Reload()
        {
            list.BeginUpdate();
            list.Items.Clear();
            var st = Store();
            try
            {
                st.Open(OpenFlags.ReadOnly);
                foreach (var c in st.Certificates)
                {
                    var it = new ListViewItem(Name(c));
                    it.SubItems.Add(c.Thumbprint);
                    it.Tag = c.Thumbprint;
                    list.Items.Add(it);
                    c.Dispose();
                }
                SetStatus("Заблокировано: " + list.Items.Count, false);
            }
            catch (Exception ex) { SetStatus("Ошибка: " + ex.Message, true); }
            finally { st.Close(); list.EndUpdate(); }
        }

        void BlockFromFile()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Сертификаты (*.cer;*.crt;*.der;*.pem)|*.cer;*.crt;*.der;*.pem|Все файлы (*.*)|*.*";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                X509Certificate2 pub = null;
                var st = Store();
                try
                {
                    using (var loaded = new X509Certificate2(dlg.FileName))
                        pub = new X509Certificate2(loaded.RawData);
                    st.Open(OpenFlags.ReadWrite);
                    bool exists = false;
                    foreach (var c in st.Certificates) { if (c.Thumbprint == pub.Thumbprint) exists = true; c.Dispose(); }
                    if (exists) { SetStatus("Уже заблокирован.", false); return; }
                    st.Add(pub);
                    SetStatus("Заблокировано: " + Name(pub), false);
                }
                catch (Exception ex) { SetStatus("Ошибка: " + ex.Message, true); }
                finally { st.Close(); if (pub != null) pub.Dispose(); Reload(); }
            }
        }

        void Unblock()
        {
            if (list.SelectedItems.Count == 0) { SetStatus("Выберите сертификат.", true); return; }
            var thumb = (string)list.SelectedItems[0].Tag;
            var st = Store();
            try
            {
                st.Open(OpenFlags.ReadWrite);
                foreach (var c in st.Certificates)
                    if (c.Thumbprint == thumb) { st.Remove(c); break; }
                SetStatus("Разблокировано.", false);
            }
            catch (Exception ex) { SetStatus("Ошибка: " + ex.Message, true); }
            finally { st.Close(); Reload(); }
        }
    }
}
