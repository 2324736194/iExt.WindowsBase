using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

using Prism.Commands;

using Cursor = System.Windows.Forms.Cursor;
using Image = System.Windows.Controls.Image;

namespace iExt.WindowsBase.Demo.ViewModels.Interop
{
    internal class InteropHandlerViewModel
    {
        private Form _form;

        public ICommand SetFormCursorCommand { get; }

        public ICommand SetFormIconCommand { get; }

        public InteropHandlerViewModel()
        {
            SetFormCursorCommand = new DelegateCommand<Image>(SetFormCursorCommandExecuteMethod);
            SetFormIconCommand = new DelegateCommand<Image>(SetFormIconCommandExecuteMethod);
        }

        private void SetFormIconCommandExecuteMethod(Image image)
        {
            FormHandler(image, SetFormIconAction);
        }

        private void SetFormIconAction(Image image, Form form)
        {
            var handler = new InteropHandler();
            var ptr = handler.ToIcon(image.Source);
            var icon = Icon.FromHandle(ptr);
            form.Icon = icon;
        }

        private void FormHandler(Image image, Action<Image, Form> action)
        {
            var form = GetForm();
            action.Invoke(image, form);
            form.Activate();
        }

        private void SetFormCursorCommandExecuteMethod(Image image)
        {
            FormHandler(image, SetFormCursorAction);
        }

        private void SetFormCursorAction(Image image, Form form)
        {
            var handler = new InteropHandler();
            var ptr = handler.ToIcon(image.Source);
            var cursor = new Cursor(ptr);
            form.Cursor = cursor;
        }

        private Form GetForm()
        {
            if (null == _form)
            {
                var main = System.Windows.Application.Current.MainWindow;
                var window = new Win32Window(main);
                var form = new Form();
                form.Closed += FormOnClosed;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show(window);
                _form = form;
            }

            return _form;
        }

        private void FormOnClosed(object sender, EventArgs e)
        {
            _form = null;
        }
    }
}
