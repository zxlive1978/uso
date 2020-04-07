using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace codeserveer
{
    public class _7017
    {
        //[7017]
        //DI_CHANNEL_COUNT=0
        //DO_CHANNEL_COUNT=0
        //AI_CHANNEL_COUNT=8
        //AO_CHANNEL_COUNT=0
        //COUNT_FREQ_CHANNEL_COUNT=0
        //DESCRIPTION=8*AI (mA,mV,V)
        //FORMDLL=UI_VC
        //NAMESPACE=ICPDAS
        //CLASS=AI_VC
        //LANGUAGE=SAVED
        //Номер порта
        public byte com=3;
        //Адрес
        public short address=7;
        //Скорость
        public uint baudrate=57600;
        //Слот
        public short slot=-1;
        //Контрольная сумма
        public short checksum=0;
        //Таймаут
        public short timeout=200;
        //Количество каналов
        public short totalchanels=8;
        //Тип канала
        public string chanels_type = "AI";
        //Имя устройства
        public string name = "";
        //Значения каналов
        public short[] AIValue = new short[8];
        //Состояние устройства
        public short iRet;

        //Список каналов
        public List<string> list_channels = new List<string>();

        


        public _7017(){
        
        }
    }
}
