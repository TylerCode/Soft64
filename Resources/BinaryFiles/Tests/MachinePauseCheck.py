from System import *
from Soft64 import *
from NLog import *

logger = LogManager.GetLogger("Machine Status Checker Script")
logger.Info(String.Format("Machine is paused: {0}", Machine.Current.IsPaused == True))