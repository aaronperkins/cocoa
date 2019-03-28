#include "ServoControllerAx12A.h"

ServoControllerAx12A::ServoControllerAx12A(unsigned char servoCount, long baudRate, unsigned char directionPin, HardwareSerial &serial)
	:ServoController(servoCount),_baudRate(baudRate), _directionPin(directionPin), _serial(serial)
{
	_ax12Position = new int[servoCount];
}

void ServoControllerAx12A::begin()
{
	HardwareSerial *serial = &_serial;
	ax12a.begin(_baudRate, _directionPin, serial);

	getCurrentPosition(_servoTrack, 0, _servoCount);
}

void ServoControllerAx12A::setTorque(bool enabled)
{
	ax12a.torqueStatus(BROADCAST_ID, enabled);
}

void ServoControllerAx12A::setTorque(unsigned char id, bool enabled)
{
	ax12a.torqueStatus(id, enabled);
}

bool ServoControllerAx12A::setNextPosition(ServoTrack track[], unsigned char count)
{
	for (unsigned char i = 0; i < count; i++)
	{
		if (track[i].nextAngle < 0)
			_ax12Position[i] = -1;
		else
			_ax12Position[i] = min(radiansToRaw(track[i].nextAngle), MAX_POS);
	}

	ax12a.moveSync(_ax12Position, count);

	return true;
}

void ServoControllerAx12A::getCurrentPosition(ServoTrack track[], unsigned char start, unsigned char count)
{
	int pos = -1;
	for (unsigned char i = start; i < count; i++)
	{
		pos = ax12a.readPosition(i);
		if (pos >= 0)
			track[i].currentAngle = rawToRadians(pos);
	}
}

int ServoControllerAx12A::radiansToRaw(float radians)
{
	while (radians >= TWO_PI) radians -= TWO_PI;
	while (radians < 0) radians += TWO_PI;

	return  (int)(radians * RAD_TO_RAW + 0.5);
}

float ServoControllerAx12A::rawToRadians(int raw)
{
	return raw * (RAW_TO_RAD);
}