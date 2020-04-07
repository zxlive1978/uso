using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace codeserveer
{
    public class _7080
    {

        //[7080]
        //DI_CHANNEL_COUNT=0
        //DO_CHANNEL_COUNT=2
        //AI_CHANNEL_COUNT=0
        //AO_CHANNEL_COUNT=0
        //COUNT_FREQ_CHANNEL_COUNT=2
        //DESCRIPTION=2*Counter/Frequency + 2*DO
        //FORMDLL=UI_CntFreq
        //NAMESPACE=ICPDAS
        //CLASS=CounterFreq
        //LANGUAGE=SAVED

        //Номер порта
        public byte com = 3;
        //Адрес
        public short address = 7;
        //Скорость
        public uint baudrate = 57600;
        //Слот
        public short slot = -1;
        //Контрольная сумма
        public short checksum = 0;
        //Таймаут
        public short timeout = 50;
        //Количество каналов
        public short totalchanels = 2;
        //Тип канала
        public string chanels_type = "DIO";
        //Имя устройства
        public string name = "";
        //Счетчик 0
        public long Value_0_cur;
        //Счетчик 1
        public long Value_1_cur;
        //Счетчик 0 старое
        public long Value_0_old;
        //Счетчик 1
        public long Value_1_old;
        //Базовое смещение
        public long Value_Base=100000;
        //Результат тальблока
        public long Value_Result = 100000;
        //Состояние устройства
        public short iRet;

        //Посылка Считывание
        //Команда
        public byte[] send_0_channel = new byte [5];
        public byte[] send_1_channel = new byte[5];
        //Ответ
        public byte[] recieve_0_channel = new byte[9];
        public byte[] recieve_1_channel = new byte[9];
        //Время окончания
        public uint wt = 50;
        //Время опроса
        public uint time = 50;
        //Cтоповый бит
        public uint stop = 0;

        //Список каналов
        public List<string> list_channels = new List<string>();

        //

        public _7080() { 
        
        }
    }
}
