//WDによる自動復帰
//参考URL
//http://tetsuakibaba.jp/index.php?page=workshop/ArduinoBasis/sleep
//https://playground.arduino.cc/Learning/ArduinoSleepCode/
#include <avr/wdt.h>
#include <avr/interrupt.h>
#include <avr/sleep.h>

#include <avr/io.h>        // Adds useful constants
#include <util/delay.h>    // Adds delay_ms and delay_us functions
#include <SoftwareSerial.h>

//#define WD_COUNT (450 * 3)  //死活通信実行回数(×8秒周期)　450回で1時間
#define WD_COUNT 1

#define LED1 PB0

int wdCounter = WD_COUNT;  //WD起動回数カウンタ
bool trapped = false;

/* 必須　*/
/*WDTがRESETを繰り返す異常を防ぐためのコード */
uint8_t mcusr_mirror __attribute__ ((section (".noinit")));
void get_mcusr(void) __attribute__((naked)) __attribute__((section(".init3")));

void get_mcusr(void){
  mcusr_mirror = MCUSR;
  MCUSR = 0;
  wdt_disable();
}
/* WDTがRESETを繰り返す異常を防ぐためのコード */

//PB0 : Rx
//PB1 : Tx
SoftwareSerial serial(PB3, PB4); // RX, TX

void wdt_set(void);
void notifyTrapped(void);
void sendWatchDog(void);

// 割り込みマクロ(Watch dog)
ISR(WDT_vect) {
  
  // ウォッチドッグ停止
  wdt_disable();

  /* 処理内容 */
  wdCounter--;

  // ウォッチドッグ再設定
  wdt_set();
}

// 割り込みマクロ(INT0)
ISR(INT0_vect){

  //罠通知フラグセット
  trapped = true;
  
}


void setup()  { 

  //ソフトウェアシリアル開始
  //serial.begin(9600);

  pinMode(INT0, INPUT);

  //Debug
  pinMode(LED1, OUTPUT);

  // Low consumption setting
  // 1. disable ADC（A/D変換器）
  ADCSRA &= ~(1 << ADEN);         // OFF  (ADCSRA & 01111111)   
  // 2. disable ACSR（アナログ比較器）
  ACSR |= (1 << ACD);             // OFF
  // 3. WatchDogTimer Setting
  wdt_set();
  // 4. Switch to the Power Down sleep mode
  set_sleep_mode(SLEEP_MODE_PWR_DOWN);

  if(serial.available()){
    serial.print("Smart Trap Controller started");
    serial.println();
  }
} 

void notifyTrapped(){
  if(serial.available()){
    serial.print(0x00);
    serial.print(0x01);
    serial.print(0x02);
    serial.println();
  }
}
void sendWatchDog(){
  if(serial.available()){
    serial.print(0xF0);
    serial.print(0xF1);
    serial.print(0xF2);
    serial.println();
  }
}

void loop() {

  if(serial.available()){
    serial.print("Loop WatchdogCounter = ");
    serial.print(wdCounter);
    serial.print(" / Trapped = ");
    serial.print(trapped);
    serial.println();
  }

  
  //ウォッチドッグ送信
  if(wdCounter == 0){
    wdCounter = WD_COUNT;
    sendWatchDog();
    digitalWrite(LED1, HIGH);
    delay(500);
    digitalWrite(LED1, LOW);
    delay(500);
    digitalWrite(LED1, HIGH);
    delay(500);
    digitalWrite(LED1, LOW);
    
  }

  //罠通知
  if(trapped){
    trapped = false;
    
    digitalWrite(LED1, HIGH);
    delay(500);
    digitalWrite(LED1, LOW);
    delay(500);
    digitalWrite(LED1, HIGH);
    delay(500);
    digitalWrite(LED1, LOW);
    delay(500);
    digitalWrite(LED1, HIGH);
    delay(500);
    digitalWrite(LED1, LOW);
    
    notifyTrapped();
  }
  
  sleep_enable();
  sleep_cpu();  //スリープ状態へ遷移

  /***********************************************************/
 
  //起動直後にSLEEP解除
  sleep_disable();

}


// WDT setting
void wdt_set(void) {
  // WDT Initialize (Reset)
  cli();                      //clear all interrupts
  wdt_reset();

  /*
   * bit0-2:WDP0-WDP2(ウォッチドッグ間隔設定ビット0～2)
   * bit5:WDP3(ウォッチドッグ間隔設定ビット3)
   * bit3:WDE(ウォッチドッグ許可　1:許可　0:禁止)
   * bit4:WDCE(ウォッチドッグ変更許可　1:許可　0:禁止　WDEの設定に必要　ハードウェアが4クロック周期後0にする)
   * bit6:WDIE(ウォッチドッグ割り込み許可)
   * bit7:WDIF(ウォッチドッグ割り込み要求フラグ　ISRマクロで自動的に制御される為意識する必要ない)
   */

  //1.同じ操作でWDCEとWDEを1にします(WDCEはH/Wにより4クロック周期後に自動的に0に戻る)
  WDTCR = (1 << WDCE)|(1 << WDE);
  //2.WDEとWDP0～3を書込みます(WDCEは解除されているが問題ない)
  WDTCR |= (1 << WDE)|(1 << WDP3)|(0 << WDP2)|(0 << WDP1)|(1 << WDP0);    //8s sleep
  //3.ウォッチドッグ割り込み許可設定　未設定の場合リセットとなる／ウォッチドッグ割り込み後に1にしないとreturn後にリセットされてしまうので注意
  WDTCR |= (1 << WDIE);

  //Set Inturrupt
  sei();
}

 




