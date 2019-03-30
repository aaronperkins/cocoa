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
            overlay={(value * 180.0 / Math.PI).toFixed(2)}
            visible={dragging}
            placement="top"
            key={index}
        >
            <Handle value={value} {...restProps} />
        </Tooltip>
    );
};

export class PoseInput extends Component {
    constructor(props) {
        super(props);

        this.handleOnChange = this.handleOnChange.bind(this);
        this.handleOnAfterChange = this.handleOnAfterChange.bind(this);
    }    

    handleOnChange(id, value) {
        this.props.onChange(id, value);
    }

    handleOnAfterChange(id, value) {
        this.props.onAfterChange(id, value);
    }

    render() {
        const angles = this.props.value;
        const sliderStyle = ["mb-2"];
        const sliderStep = 0.001;
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
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[4]} handle={handle}
                            onChange={(value) => this.handleOnChange(4, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(4, value)}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Shoulder (2)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[1]} handle={handle}
                            onChange={(value) => this.handleOnChange(1, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(1, value)}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <label>Upper (5)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[5]} handle={handle}
                            onChange={(value) => this.handleOnChange(5, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(5, value)}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Upper (2)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[2]} handle={handle}
                            onChange={(value) => this.handleOnChange(2, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(5, value)}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <label>Lower (6)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[6]} handle={handle}
                            onChange={(value) => this.handleOnChange(6, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(6, value)}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Lower (3)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[3]} handle={handle}
                            onChange={(value) => this.handleOnChange(3, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(3, value)}
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
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[7]} handle={handle}
                            onChange={(value) => this.handleOnChange(7, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(7, value)}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Shoulder (10)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[10]} handle={handle}
                            onChange={(value) => this.handleOnChange(10, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(10, value)}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <label>Upper (8)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[8]} handle={handle}
                            onChange={(value) => this.handleOnChange(8, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(8, value)}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Upper (11)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[11]} handle={handle}
                            onChange={(value) => this.handleOnChange(11, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(11, value)}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <label>Lower (9)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[9]} handle={handle}
                            onChange={(value) => this.handleOnChange(9, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(9, value)}
                        />
                    </Col>
                    <Col></Col>
                    <Col>
                        <label>Lower (12)</label>
                        <Slider className={sliderStyle} step={sliderStep} min={0} max={sliderMax} value={angles[12]} handle={handle}
                            onChange={(value) => this.handleOnChange(12, value)}
                            onAfterChange={(value) => this.handleOnAfterChange(12, value)}
                        />
                    </Col>
                </Row>
            </Container>
        );
    }
}
