import React, { Component } from 'react';
import { TransactionsClient, ParseCsvModel,  TransactionModel } from '../../../firewatch.service';
import { StyleSheet, css } from 'aphrodite';
import { FormGroup, Radio, RadioGroup, FileInput, Button } from '@blueprintjs/core';

interface ImportCsvProps {
    onParseResults?(transactions: TransactionModel[]): void;
}

interface ImportCsvState {
  csvContents?: string;
  selectedBankFormat?: string;
  supportedBankFormats: SupportedBankFormat[];
}

type SupportedBankFormat = { abbreviation: string, description: string };


export class ImportCsv extends Component<ImportCsvProps, ImportCsvState> {

  constructor(props: ImportCsvProps) {
    super(props);

    this.state = {
        supportedBankFormats: [],
    };

    this.readFile = this.readFile.bind(this);
    this.submit = this.submit.bind(this);
    this.onLoad = this.onLoad.bind(this);
    this.handleSelectionChange = this.handleSelectionChange.bind(this);
  }

  componentDidMount() {
      // TODO: this is stubbing some functionality that doesn't yet exist.
      // Eventually we will fetch the supported formats from the API.
      this.setState({
          ...this.state,
          supportedBankFormats: [ 
            { abbreviation: 'RBC', description: 'Royal Bank of Canada CSV' } 
          ],
          selectedBankFormat: 'RBC',
      })
  }

  onLoad(event: ProgressEvent<FileReader>) {
    const content = event.target?.result?.toString() ?? "";
    this.setState({
        csvContents: content,
    });
  }

  readFile(e: React.FormEvent<HTMLInputElement>) {
    if (e.target && e.currentTarget.files && e.currentTarget.files.length > 0) {
      const fileReader = new FileReader();
      fileReader.onload = this.onLoad;
      fileReader.readAsText(e.currentTarget.files[0]);
    }
  }

  get canSubmit(): boolean {
    const canSubmit = (this.state.csvContents && this.state.selectedBankFormat) ? true : false;
    return canSubmit;
  }

  submit() {
    if (this.state.csvContents) {
        const client = new TransactionsClient();
        var body = new ParseCsvModel();
        body.csv = this.state.csvContents;
        body.bank = this.state.selectedBankFormat; // TODO
        client.parseCsv(body).then(response => {
            if (this.props.onParseResults) {
                this.props.onParseResults(response.transactions || []);
            } else {
                console.log('Received unhandled parse results', response.transactions);
            }
        });
    }
  }

  handleSelectionChange(event: React.FormEvent<HTMLInputElement>) {
    this.setState({
        ...this.state,
        selectedBankFormat: event.currentTarget?.value,
    });
  }

  render() {

        const supportedFormatOptions: JSX.Element[] = [];
        for (let format of this.state.supportedBankFormats) {
            supportedFormatOptions.push(<option value={format.abbreviation}>{format.description}</option>);
        }

        const radioButtons: JSX.Element[] = [];
        for (let opt of this.state.supportedBankFormats) {
            radioButtons.push(
                // <FormGroup  key={opt.abbreviation}>
                //     <Label check >
                //         <Input type="radio" 
                //             value={opt.abbreviation} 
                //             checked={this.state.selectedBankFormat === opt.abbreviation} 
                //             onChange={this.handleSelectionChange} />{' '}{opt.description}
                //     </Label>
                // </FormGroup>
                <Radio label={opt.description} value={opt.abbreviation} key={opt.abbreviation} />
            );
        }

      return (
        <form>
            <FileInput text="Choose file..." onInputChange={this.readFile} />

            <RadioGroup 
              label="Parsing format"
              selectedValue={this.state.selectedBankFormat}
              onChange={this.handleSelectionChange}
              >
                {radioButtons}
            </RadioGroup>
            <Button disabled={!this.canSubmit} onClick={this.submit} intent="primary">Upload</Button>
        </form>);
  }
}