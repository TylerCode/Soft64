from Soft64 import Machine
from System import String
from System.Windows.Forms import MessageBox

MessageBox.Show(String.Format("Machine Paused: {0}", Machine.Current.IsPaused == True))