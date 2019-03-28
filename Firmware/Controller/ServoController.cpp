#include "ServoController.h"

ServoController::ServoController(unsigned char servoCount)
	:_servoCount(servoCount)
{
	 _servoTrack = new ServoTrack[servoCount];
}

void ServoController::begin(){}

void ServoController::setTorque(bool enabled) {}

void ServoController::setTorque(unsigned char id, bool enabled){}

bool ServoController::setNextPosition(ServoTrack track[], unsigned char count){return false;}

void ServoController::getCurrentPosition(ServoTrack track[], unsigned char start, unsigned char count){}

void ServoController::move(unsigned char id, float position, unsigned int duration)
{
	startMove(_servoTrack, id, position, duration);
}

void ServoController::loop()
{
	getCurrentPosition(_servoTrack, 1, _servoCount);

	doFrame(_servoTrack, _servoCount);
}

bool ServoController::isMoving()
{
	for (int i = 0; i < _servoCount; i++)
	{
		if (_servoTrack[i].moving)
		{
			return true;
		}

	}
	return false;
}

void ServoController::torque(bool enabled)
{
	setTorque(enabled);
}

void ServoController::torque(unsigned char id, bool enabled)
{
	setTorque(id, enabled);
}

unsigned char ServoController::servoCount()
{
	return _servoCount;
}

ServoTrack *ServoController::servoTrack()
{
	return _servoTrack;
}

void ServoController::doFrame(ServoTrack track[], unsigned char count)
{
	bool pushFrame = false;

	for (int i = 0; i < count; i++)
	{
		if (!track[i].moving)
		{
			continue;
		}

		if (abs(track[i].currentAngle - track[i].targetAngle) < 0.04
			|| track[i].elapsedTime > (track[i].duration + 25))
		{
			track[i].moving = false;
			continue;
		}

		track[i].speed = min((float)track[i].elapsedTime / (float)track[i].duration, 1.0);

		track[i].nextAngle = track[i].startAngle + (track[i].distance * track[i].speed);
		pushFrame = true;
	}

	if (pushFrame)
	{
		setNextPosition(track, count);
	}

	for (int i = 0; i < count; i++)
	{
		if (track[i].moving)
		{
			track[i].elapsedTime = millis() - track[i].startTime;
		}
	}
}

void ServoController::startMove(ServoTrack track[], unsigned char id, float position, unsigned int duration)
{
	track[id].startAngle = track[id].currentAngle;
	track[id].targetAngle = position;
	track[id].distance = track[id].targetAngle - track[id].startAngle;
	track[id].duration = duration;
	track[id].startTime = millis();
	track[id].elapsedTime = 0;
	track[id].moving = true;
}