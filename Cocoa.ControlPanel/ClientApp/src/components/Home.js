import 'rc-slider/assets/index.css';
import 'rc-tooltip/assets/bootstrap.css';
import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import Tooltip from 'rc-tooltip';
import Slider, { Range } from 'rc-slider';

const createSliderWithTooltip = Slider.createSliderWithTooltip;
const Handle = Slider.Handle;
const handle = (props) => {
    const { value, dragging, index, ...restProps } = props;
    return (
        <Tooltip
            prefixCls="rc-slider-tooltip"
            overlay={value * 180.0 / Math.PI}
            visible={dragging}
            placement="top"
            key={index}
        >
            <Handle value={value} {...restProps} />
        </Tooltip>
    );
};

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.state = {
            angles: [-1.0, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI]
        };

        this.getCurrentAngles();
    }    

    getCurrentAngles() {
        fetch('api/command/pose')         
            .then(response => response.json())
            .then(data => {
                for (var key in data.joints) {
                    var id = parseInt(data.joints[key].id);
                    var angle = parseFloat(data.joints[key].angle);
                    this.changeAngle(id, angle);                  
                }
            });    

        this.changeAngle(1, this.state.angles[1] + 0.1);
    }

    changeAngle(i, value) {
        this.setState(state => {
            const angles = state.angles.map((item, j) => {
                if (j === i) {
                    return value;
                } else {
                    return item;
                }
            });

            return {
                angles,
            };
        });
    };

    pose() {
        var pose = { id: 23, joints: [] };

        for (var key in this.state.angles) {
            pose.joints.push({ id: parseInt(key), angle: this.state.angles[key], duration: 1000 });
        }

        fetch('api/command/pose', {
            method: 'post',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(pose)
        })
    }

    stand() {
        var opts = { hey: "blah", what: "hmm" };
        fetch('api/command/stand', {
            method: 'post',
            body: JSON.stringify(opts)
        })
    }

    lay() {
        var opts = { hey: "blah", what: "hmm" };
        fetch('api/command/lay', {
            method: 'post',
            body: JSON.stringify(opts)
        })
    }

    render() {
        const sliderStyle = ["mb-2"];
        const sliderStep = 0.1;
        const sliderMax = 2.0 * Math.PI;
        return (
            <Container>

                <Row>
                    <Col><h4>Front Left</h4></Col>
                    <Col></Col>
                    <Col><h4>Front Right</h4></Col>
                </Row>
                <Row>
                    <Col><hr /></Col>
                </Row>
                <Row>
                    <Col>
                        <label>Shoulder (4)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[4]} handle={handle} onChange={(value) => this.changeAngle(4, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Shoulder (2)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[1]} handle={handle} onChange={(value) => this.changeAngle(1, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <label>Upper (5)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[5]} handle={handle} onChange={(value) => this.changeAngle(5, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Upper (2)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[2]} handle={handle} onChange={(value) => this.changeAngle(2, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <label>Lower (6)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[6]} handle={handle} onChange={(value) => this.changeAngle(6, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Lower (3)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[3]} handle={handle} onChange={(value) => this.changeAngle(3, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col><hr /></Col>
                </Row>
                <Row>
                    <Col><h4>Rear Left</h4></Col>
                    <Col></Col>
                    <Col><h4>Rear Right</h4></Col>
                </Row>
                <Row>
                    <Col><hr /></Col>
                </Row>
                <Row>
                    <Col>
                        <label>Shoulder (7)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[7]} handle={handle} onChange={(value) => this.changeAngle(7, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Shoulder (10)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[10]} handle={handle} onChange={(value) => this.changeAngle(10, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <label>Upper (8)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[8]} handle={handle} onChange={(value) => this.changeAngle(8, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Upper (11)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[11]} handle={handle} onChange={(value) => this.changeAngle(11, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <label>Lower (9)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[9]} handle={handle} onChange={(value) => this.changeAngle(9, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Lower (12)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={this.state.angles[12]} handle={handle} onChange={(value) => this.changeAngle(12, value)}
                            onAfterChange={() => this.pose()}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col><button className="btn btn-primary" onClick={this.stand}>Stand</button></Col>
                    <Col><button className="btn btn-primary" onClick={this.lay}>Lay Down</button></Col>
                    <Col><button className="btn btn-primary" onClick={() => this.getCurrentAngles()}>Capture</button></Col>
                </Row>
            </Container>
        );
    }
}
