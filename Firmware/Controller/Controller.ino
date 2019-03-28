#include "wiring_private.h"
#include "ServoControllerAx12A.h"
#include "PoseController.h"

#pragma region serial setup
Uart Serial2(&sercom5, 13, 12, SERCOM_RX_PAD_1, UART_TX_PAD_0); // 13(D13 =>PA22 => Sercom 5.1) , 12(D12 =>PA23 => Sercom 5.0)

void SERCOM5_0_Handler()
{
	Serial2.IrqHandler();
}
void SERCOM5_1_Handler()
{
	Serial2.IrqHandler();
}

void SERCOM5_2_Handler()
{
	Serial2.IrqHandler();
}

void SERCOM5_3_Handler()
{
	Serial2.IrqHandler();
}
#pragma endregion

const unsigned char SERVO_COUNT = 13;
const byte RECEIVE_BUFFER_SIZE = 255;

HardwareSerial &_servoSerial = Serial1;
Serial_ &_commandSerial = Serial;
//HardwareSerial &_commandSerial = Serial2;

ServoControllerAx12A _servoController(13, 1000000, 2, _servoSerial);
PoseController _poseController(_servoController);
char _receiveBuffer[RECEIVE_BUFFER_SIZE];
unsigned long _frameStartTime = 0;
unsigned long _lastFrameTime = 0;
double _fps = 0.0;

void setup()
{
	// set up serial 2 pins
	pinPeripheral(12, PIO_SERCOM_ALT);
	pinPeripheral(13, PIO_SERCOM_ALT);

	_commandSerial.begin(115200);

	_servoController.begin();

	_poseController.setPoseCompleteCallback(poseCompleteCallback);
}

void loop()
{
	_frameStartTime = micros();

	receiveData(_commandSerial);

	_poseController.loop();

	_servoController.loop();

	_lastFrameTime = micros() - _frameStartTime;
	_fps = 1000000.00 / _lastFrameTime;
}

void poseCompleteCallback(int poseId)
{
	writePoseFinish(_servoController.servoTrack(), SERVO_COUNT, _commandSerial, poseId);
}

void receiveData(Stream &serial)
{
	static boolean recvInProgress = false;
	static byte ndx = 0;
	static bool newData = false;
	static char receiveParseBuffer[RECEIVE_BUFFER_SIZE];
	char const startMarker = '<';
	char const endMarker = '>';
	char rc;

	while (serial.available() > 0 && newData == false) {
		rc = serial.read();

		if (recvInProgress == true) {
			if (rc != endMarker) {
				_receiveBuffer[ndx] = rc;
				ndx++;
				if (ndx >= RECEIVE_BUFFER_SIZE) {
					ndx = RECEIVE_BUFFER_SIZE - 1;
				}
			}
			else {
				_receiveBuffer[ndx] = '\0';
				recvInProgress = false;
				ndx = 0;
				newData = true;
			}
		}

		else if (rc == startMarker) {
			recvInProgress = true;
		}
	}

	if (newData == true) {
		strcpy(receiveParseBuffer, _receiveBuffer);
		parseData(receiveParseBuffer, serial);
		newData = false;
	}
}

void parseData(char *buffer, Stream &serial)
{
	char *strtokIndx;
	char instruction;

	strtokIndx = strtok(buffer, ",");
	instruction = strtokIndx[0];

	switch (instruction) {
	case 'P':
	{
		struct Pose pose;
		pose.joint = new Joint[SERVO_COUNT];

		strtokIndx = strtok(NULL, ",");
		pose.id = atoi(strtokIndx);
		for (int i = 0; i < SERVO_COUNT; i++)
		{
			strtokIndx = strtok(NULL, ",");
			pose.joint[i].angle = atof(strtokIndx);
			strtokIndx = strtok(NULL, ",");
			pose.joint[i].duration = atoi(strtokIndx);
		}

		_poseController.queuePose(pose);
	}
	break;

	case 'C':
		writePose(_servoController.servoTrack(), SERVO_COUNT, serial);
		break;

	case 'R':
		_servoController.torque(false);
		break;

	case 'S':
		_servoController.torque(true);
		break;

	case 'T':
	{
		unsigned char id;
		bool val;
		strtokIndx = strtok(NULL, ",");
		id = atoi(strtokIndx);
		strtokIndx = strtok(NULL, ",");
		val = atoi(strtokIndx);
		_servoController.torque(id, val);
	}
		break;

	case 'D':
		serial.println("\r");
		debugServoPositions(_servoController.servoTrack(), 1, SERVO_COUNT, _commandSerial);
		break;
	}
}

void writePose(ServoTrack track[], unsigned char count, Stream &serial)
{
	serial.print("<P,");
	serial.print(0);
	serial.print(",");
	for (int i = 0; i < count; i++)
	{
		serial.print(track[i].currentAngle, 4);
		serial.print(",");
		serial.print(1000);
		if (i < count - 1)
			serial.print(",");
	}
	serial.print(">");
}

void writePoseFinish(ServoTrack track[], unsigned char count, Stream &serial, int id)
{
	serial.print("<F,");
	serial.print(id);
	serial.print(",");
	for (int i = 0; i < count; i++)
	{
		serial.print(track[i].currentAngle, 4);
		serial.print(",");
		serial.print(track[i].elapsedTime);
		if (i < count - 1)
			serial.print(",");
	}
	serial.print(">");
}

void debugServoPositions(ServoTrack track[], unsigned char start, unsigned char count, Stream &serial)
{
	serial.print("Mem: ");
	serial.print(freeMemory());
	serial.print("\t");
	serial.print("Fps: ");
	serial.print(_fps);
	serial.print("\t");
	for (int i = start; i < count; i++)
	{
		serial.print(i);
		serial.print(": T");
		serial.print(track[i].targetAngle);
		serial.print(" C");
		serial.print(track[i].currentAngle);
		serial.print(" E");
		if (track[i].elapsedTime < 1000) serial.print('0');
		if (track[i].elapsedTime < 100) serial.print('0');
		if (track[i].elapsedTime < 10) serial.print('0');
		serial.print(track[i].elapsedTime);
	}
	serial.print("\n\r");
}

#ifdef __arm__
// should use uinstd.h to define sbrk but Due causes a conflict
extern "C" char* sbrk(int incr);
#else  // __ARM__
extern char *__brkval;
#endif  // __arm__

int freeMemory() {
	char top;
#ifdef __arm__
	return &top - reinterpret_cast<char*>(sbrk(0));
#elif defined(CORE_TEENSY) || (ARDUINO > 103 && ARDUINO != 151)
	return &top - __brkval;
#else  // __arm__
	return __brkval ? &top - __brkval : &top - __malloc_heap_start;
#endif  // __arm__
}